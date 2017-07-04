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
                Some <| R.button
                    [P.OnClick (fun _ -> dispatch (Flip continuation))]
                    [R.str <| sprintf "Attempt (%A/%A against target %d)" skill attr target]
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
            if List.forall (fun flag -> Set.contains flag gameState.Flags) flags then
                makeConditionButton continuation nextCondition gameState dispatch
            else
                None

let view (gameState : ActiveGameState) (result : Skills.RollResult option) dispatch =
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
        R.div [P.ClassName "storyArea"] [
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
            for paragraph in page.Text ->
                R.p [] [Modal.formatLine paragraph dispatch]
            yield R.ul [] [
                for cont in page.Continuations do
                    match makeConditionButton cont cont.Condition gameState dispatch with
                    | None -> ()
                    | Some conditionButton ->
                        yield R.li [] [
                            yield R.str cont.Description
                            yield R.br []
                            yield conditionButton
                    ]
            ]
        ]
    ]