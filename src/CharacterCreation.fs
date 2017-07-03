module Wanderer.CharacterCreation

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.Model
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

[<Emit("$0.options[$0.selectedIndex].value")>]
let private getSelectedValue (srcElement : Element) : string = jsNative

let view character dispatch =
    R.div [] [
        R.h1 [] [R.str "The Wanderer"]
        para """
            So you wish to know the full story, stranger?  Well, as long as the coin for my drinks keeps coming out
            of your purse, I will keep talking.
            """
        R.p [] [
            R.str """
                To other humans, my name is Pompeia.  To your kind, I am known only as wanderer.  I have traveled far over
                these lands, and
                """
            R.select
                [P.OnChange (fun e ->
                    let idx : int = unbox e.nativeEvent.srcElement?selectedIndex
                    match idx with
                    | 0 -> { character with Might = 3; Will = 3 }
                    | 1 -> { character with Might = 2; Will = 4 }
                    | 2 -> { character with Might = 4; Will = 2 }
                    | i ->
                        printfn "Could not recognive index %d" i
                        character
                    |> Model.UpdateCharacter
                    |> dispatch)]
                [
                    R.option [] [R.str "my body and mind are in balance"]
                    R.option [] [R.str "my mind is unmatched"]
                    R.option [] [R.str "my strength is known of far and wide"]
                ]
            R.str ". When confronted with an obstacle, my first response is always to "
            R.select
                [P.OnChange (fun e ->
                    let idx : int = unbox e.nativeEvent.srcElement?selectedIndex
                    match idx with
                    | 0 ->
                        let newLowSkill = if character.LowSkill <> Persuasion then character.LowSkill else Combat
                        { character with HighSkill = Persuasion; LowSkill = newLowSkill }
                    | 1 ->
                        let newLowSkill = if character.LowSkill <> Combat then character.LowSkill else Persuasion
                        { character with HighSkill = Combat; LowSkill = newLowSkill }
                    | 2 ->
                        let newLowSkill = if character.LowSkill <> Ritual then character.LowSkill else Persuasion
                        { character with HighSkill = Ritual; LowSkill = newLowSkill }
                    | 3 ->
                        let newLowSkill = if character.LowSkill <> Sneaking then character.LowSkill else Persuasion
                        { character with HighSkill = Sneaking; LowSkill = newLowSkill }
                    | i ->
                        printfn "Could not recognive index %d" i
                        character
                    |> Model.UpdateCharacter
                    |> dispatch)]
                [
                    R.option [] [R.str "talk my way out"]
                    R.option [] [R.str "reach for my sword"]
                    R.option [] [R.str "prepare a spell"]
                    R.option [] [R.str "sneak past"]
                ]
            R.str ". My greatest weakness is my low skill at "
            R.select
                [P.OnChange (fun e ->
                    let value = getSelectedValue e.nativeEvent.srcElement
                    match value with
                    | "persuasion" -> { character with LowSkill = Persuasion }
                    | "combat" -> { character with LowSkill = Combat }
                    | "ritual" -> { character with LowSkill = Ritual }
                    | "sneaking" -> { character with LowSkill = Sneaking }
                    | s ->
                        printfn "Could not recognive value %s" s
                        character
                    |> Model.UpdateCharacter
                    |> dispatch)]
                [
                    if character.HighSkill <> Persuasion then
                        yield R.option [P.Value <| U2.Case1 "persuasion"] [R.str "talking to people"]
                    if character.HighSkill <> Combat then
                        yield R.option [P.Value <| U2.Case1 "combat"] [R.str "fighting"]
                    if character.HighSkill <> Ritual then
                        yield R.option [P.Value <| U2.Case1 "ritual"] [R.str "using magic"]
                    if character.HighSkill <> Sneaking then
                        yield R.option [P.Value <| U2.Case1 "sneaking"] [R.str "athletics"]
                ]
        ]
        R.h3 [] [R.str "With these choices, Pompeia will have the following stats:"]
        R.table [] [
            R.tbody [] [
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Might"]
                    R.td [] [R.str <| string character.Might]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Will"]
                    R.td [] [R.str <| string character.Will]
                ]
                R.tr [] [R.td [P.ColSpan 2.] [R.hr []]]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Persuasion"]
                    R.td [] [
                        if character.HighSkill = Persuasion then 4 else if character.LowSkill = Persuasion then 2 else 3
                        |> string
                        |> R.str
                    ]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Combat"]
                    R.td [] [
                        if character.HighSkill = Combat then 4 else if character.LowSkill = Combat then 2 else 3
                        |> string
                        |> R.str
                    ]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Ritual"]
                    R.td [] [
                        if character.HighSkill = Ritual then 4 else if character.LowSkill = Ritual then 2 else 3
                        |> string
                        |> R.str
                    ]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Sneaking"]
                    R.td [] [
                        if character.HighSkill = Sneaking then 4 else if character.LowSkill = Sneaking then 2 else 3
                        |> string
                        |> R.str
                    ]
                ]
            ]
        ]
    ]