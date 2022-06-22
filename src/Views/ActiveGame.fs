module Wanderer.ActiveGame

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Browser

open Wanderer.Modal
open Wanderer.Model
open Wanderer.Pages
open Wanderer.ViewHelpers

open Fable.React
open Fable.React.Props

let private divider =
    tr [] [td [ColSpan 2; Style [Height "1em"]] []]

let private makeStatRow title value =
    tr [] [
        th [Style [TextAlign TextAlignOptions.Right]] [str title]
        td [] [str <| string value]
    ]

let private makeInjuryRow title value =
    if value = 0 then
        tr [] []
    else
        tr [ClassName "injured"] [
            th [Style [TextAlign TextAlignOptions.Right]] [str title]
            td [] [str <| string value]
        ]

let rec private makeConditionButton continuation condition (gameState : ActiveGameState) dispatch =
        match condition with
        | Automatic ->
            Some <| button [OnClick (fun _ -> dispatch (Flip continuation))] [str "Choose"]
        | SkillCheckRequired (attr, skill, target, effect) ->
            if Character.GetAttr attr gameState.Character > 0 then
                let successChance =
                    Skills.getSuccessChance
                        { Skill = Character.GetSkill skill gameState.Character
                          Attr = Character.GetAttr attr gameState.Character }
                        target
                Some <| button
                    [OnClick (fun _ -> dispatch (Flip continuation))]
                    [str <| sprintf "%A/%A vs %d (%.0f%%)" skill attr target successChance]
            else
                let buttonText =
                    match attr with
                    | Skills.Might -> "Too many injuries."
                    | Skills.Will -> "Too much stress."
                Some <| button [Disabled true] [str buttonText]
        | Bribe cost ->
            if gameState.Character.Muld >= cost then
                Some <| button [OnClick (fun _ -> dispatch (Flip continuation))] [str <| sprintf "Bribe (%d muld)" cost]
            else
                Some <| button [Disabled true] [str "Cannot afford to bribe"]
        | Flags (flags, nextCondition) ->
            let negatedFlags, positiveFlags = List.partition (fun (f : string) -> f.StartsWith("~")) flags
            let hasAllPositiveFlags =
                List.forall (fun flag -> Set.contains flag gameState.Flags) positiveFlags
            let missingAllNegatedFlags =
                List.forall (fun (flag : string) ->
                    not (Set.contains (flag.Substring(1)) gameState.Flags)) negatedFlags
            if hasAllPositiveFlags && missingAllNegatedFlags then
                makeConditionButton continuation nextCondition gameState dispatch
            else
                None

let view (gameState : ActiveGameState) (result : Skills.RollResult option) dispatch =
    window.setTimeout(
        (fun _ ->
            window.location.href <- "#"
            window.location.href <- "#inGame"),
        0,
        [||]) |> ignore
    let character = gameState.Character
    let page = gameState.Page
    div [Style [Display DisplayOptions.Flex]] [
        table [] [
            tbody [] [
                makeStatRow "Might" character.Might
                makeStatRow "Will" character.Will
                divider
                makeStatRow "Persuasion" character.Persuasion
                makeStatRow "Combat" character.Combat
                makeStatRow "Ritual" character.Ritual
                makeStatRow "Sneaking" character.Sneaking
                divider
                makeStatRow "Muld Shells" character.Muld
                divider
                makeInjuryRow "Wounds" character.Wounds
                makeInjuryRow "Stress" character.Stress
            ]
        ]
        div [ClassName "verticalDivider"] []
        div [Id "storyArea"] [
            yield div [ClassName "history"] [
                for paragraph in gameState.History ->
                    para paragraph
            ]
            match result with
            | None -> ()
            | Some rollResult ->
                yield h4 [] [
                    yield str <| sprintf "You %s with a roll of "
                        (if rollResult.Succeeded then "succeeded" else "failed")
                    for roll in rollResult.Rolls do
                        yield div [ClassName (if roll <= rollResult.AttributeLevel then "success" else "fail")] [
                            str <| sprintf " %d " roll
                        ]
                ]
            yield div
                [Id "inGame"]
                (page.Text |> List.map (fun t -> p [] [Modal.formatLine t dispatch]))
            if List.isEmpty page.Continuations then
                yield p [] [h4 [] [str "You completed the game! Would you like to view your full log?"]]
                yield button [OnClick (fun _ -> dispatch ShowFullHistory)] [str "View full log"]
                yield str " or "
                yield a [Nowhere; OnClick (fun _ -> dispatch StartCharacterCreation)] [str "start a new game"]
            else
                let activeContinuations =
                    page.Continuations
                    |> List.choose (fun cont ->
                        match makeConditionButton cont cont.Condition gameState dispatch with
                        | None -> None
                        | Some conditionButton ->
                            Some <| li [] [
                                Modal.formatLine cont.Description dispatch
                                conditionButton
                                div [classList [("spacer", true)]] []
                            ])
                match activeContinuations with
                | [] ->
                    yield div [] [
                        h4 [] [
                            str """Uh oh, there's supposed to be a next page here, but I screwed up the flags.  Please
                                send an email to """
                            a [Href "mailto:will.pitts@outlook.com"] [str "will.pitts@outlook.com"]
                            str " and include the following information:"
                            br []
                            str <| sprintf "Page name: %s" gameState.Page.Name
                            br []
                            str <| sprintf "Flags: %A" gameState.Flags
                        ]
                    ]
                | continuations ->
                    yield ul [] continuations
        ]
    ]