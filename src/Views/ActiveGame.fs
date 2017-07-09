module Wanderer.ActiveGame

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.Modal
open Wanderer.Model
open Wanderer.Pages
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let private divider =
    R.tr [] [R.td [P.ColSpan 2.; P.Style [P.Height "1em"]] []]

let private makeStatRow title value =
    R.tr [] [
        R.th [P.Style [P.TextAlign "right"]] [R.str title]
        R.td [] [R.str <| string value]
    ]

let private makeInjuryRow title value =
    if value = 0 then
        R.tr [] []
    else
        R.tr [P.ClassName "injured"] [
            R.th [P.Style [P.TextAlign "right"]] [R.str title]
            R.td [] [R.str <| string value]
        ]

let rec private makeConditionButton continuation condition (gameState : ActiveGameState) dispatch =
        match condition with
        | Automatic ->
            Some <| R.button [P.OnClick (fun _ -> dispatch (Flip continuation))] [R.str "Choose"]
        | SkillCheckRequired (attr, skill, target, effect) ->
            if Character.GetEffectiveAttr attr gameState.Character > 0 then
                let successChance =
                    Skills.getSuccessChance
                        { Skill = Character.GetSkill skill gameState.Character
                          Attr = Character.GetAttr attr gameState.Character }
                        target
                Some <| R.button
                    [P.OnClick (fun _ -> dispatch (Flip continuation))]
                    [R.str <| sprintf "%A/%A vs %d (%.0f%%)" skill attr target successChance]
            else
                let buttonText =
                    match attr with
                    | Skills.Might -> "Too many injuries."
                    | Skills.Will -> "Too much stress."
                Some <| R.button [P.Disabled true] [R.str buttonText]
        | Bribe cost ->
            if gameState.Character.Muld >= cost then
                Some <| R.button [P.OnClick (fun _ -> dispatch (Flip continuation))] [R.str <| sprintf "Bribe (%d muld)" cost]
            else
                Some <| R.button [P.Disabled true] [R.str "Cannot afford to bribe"]
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
    R.div [P.Style [P.Display "flex"]] [
        R.table [] [
            R.tbody [] [
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
        R.div [P.ClassName "verticalDivider"] []
        R.div [P.Id "storyArea"] [
            yield R.div [P.ClassName "history"] [
                for paragraph in gameState.History ->
                    para paragraph
            ]
            match result with
            | None -> ()
            | Some rollResult ->
                yield R.h4 [] [
                    yield R.str <| sprintf "You %s with a roll of " 
                        (if rollResult.Succeeded then "succeeded" else "failed")
                    for roll in rollResult.Rolls do
                        yield R.div [P.ClassName (if roll <= rollResult.AttributeLevel then "success" else "fail")] [
                            R.str <| sprintf " %d " roll
                        ]
                ]
            yield R.div
                [P.Id "inGame"]
                (page.Text |> List.map (fun p -> R.p [] [Modal.formatLine p dispatch]))
            if List.isEmpty page.Continuations then
                yield R.p [] [R.h4 [] [R.str "You completed the game! Would you like to view your full log?"]]
                yield R.button [P.OnClick (fun _ -> dispatch ShowFullHistory)] [R.str "View full log"]
                yield R.str " or "
                yield R.a [Nowhere; P.OnClick (fun _ -> dispatch StartCharacterCreation)] [R.str "start a new game"]
            else
                yield R.ul [] [
                    for cont in page.Continuations do
                        match makeConditionButton cont cont.Condition gameState dispatch with
                        | None -> ()
                        | Some conditionButton ->
                            yield R.li [] [
                                yield Modal.formatLine cont.Description dispatch
                                yield R.br []
                                yield conditionButton
                        ]
                ]
        ]
    ]