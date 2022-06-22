module Wanderer.CharacterCreation

open Fable.Core.JsInterop

open Wanderer.Modal
open Wanderer.Model
open Wanderer.Skills
open Wanderer.ViewHelpers

open Fable.React
open Fable.React.Props

let private updateHighSkill skill fallbackSkill character =
    let newLowSkill = if character.LowSkill <> skill then character.LowSkill else fallbackSkill
    { character with HighSkill = skill; LowSkill = newLowSkill }

let view (character : InProgressCharacter) dispatch =
    div [] [
        para """
            So you wish to know the full story, stranger?  Well, as long as the coin for my drinks keeps coming out
            of your purse, I will keep talking.
            """
        p [] [
            str "To other "
            showModalLinkByName "Humans" "humans" dispatch
            str ", my name is Pompeia.  To "
            showModalLinkByName "Etzen" "Etzen" dispatch
            str ", I am known only as wanderer.  I have traveled far over these lands, and "
            select
                [OnChange (fun e ->
                    let idx : int = unbox e.target?selectedIndex
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
                    option [] [str "my body and mind are in balance"]
                    option [] [str "my mind is unmatched"]
                    option [] [str "my strength is known of far and wide"]
                ]
            str ". When confronted with an obstacle, my first response is always to "
            select
                [OnChange (fun e ->
                    let idx : int = unbox e.target?selectedIndex
                    match idx with
                    | 0 ->
                        updateHighSkill Persuasion Combat character
                    | 1 ->
                        updateHighSkill Combat Persuasion character
                    | 2 ->
                        updateHighSkill Ritual Persuasion character
                    | 3 ->
                        updateHighSkill Sneaking Persuasion character
                    | i ->
                        printfn "Could not recognive index %d" i
                        character
                    |> Model.UpdateCharacter
                    |> dispatch)]
                [
                    option [] [str "talk my way out"]
                    option [] [str "reach for my sword"]
                    option [] [str "prepare a spell"]
                    option [] [str "sneak past"]
                ]
            str ". My greatest weakness is my low skill at "
            select
                [OnChange (fun e ->
                    let value = getSelectedValue e.target
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
                        yield makeValueOption "persuasion" "talking to people" (character.LowSkill = Persuasion)
                    if character.HighSkill <> Combat then
                        yield makeValueOption "combat" "fighting" (character.LowSkill = Combat)
                    if character.HighSkill <> Ritual then
                        yield makeValueOption "ritual" "using magic" (character.LowSkill = Ritual)
                    if character.HighSkill <> Sneaking then
                        yield makeValueOption "sneaking" "athletics" (character.LowSkill = Sneaking)
                ]
        ]
        h3 [] [str "With these choices, Pompeia will have the following stats:"]
        table [] [
            tbody [] [
                tr [] [
                    th [Style [TextAlign TextAlignOptions.Right]] [str "Might"]
                    td [] [str <| string character.Might]
                ]
                tr [] [
                    th [Style [TextAlign TextAlignOptions.Right]] [str "Will"]
                    td [] [str <| string character.Will]
                ]
                tr [] [td [ColSpan 2] [hr []]]
                tr [] [
                    th [Style [TextAlign TextAlignOptions.Right]] [str "Persuasion"]
                    td [] [
                        if character.HighSkill = Persuasion then 6 else if character.LowSkill = Persuasion then 4 else 5
                        |> string
                        |> str
                    ]
                ]
                tr [] [
                    th [Style [TextAlign TextAlignOptions.Right]] [str "Combat"]
                    td [] [
                        if character.HighSkill = Combat then 6 else if character.LowSkill = Combat then 4 else 5
                        |> string
                        |> str
                    ]
                ]
                tr [] [
                    th [Style [TextAlign TextAlignOptions.Right]] [str "Ritual"]
                    td [] [
                        if character.HighSkill = Ritual then 6 else if character.LowSkill = Ritual then 4 else 5
                        |> string
                        |> str
                    ]
                ]
                tr [] [
                    th [Style [TextAlign TextAlignOptions.Right]] [str "Sneaking"]
                    td [] [
                        if character.HighSkill = Sneaking then 6 else if character.LowSkill = Sneaking then 4 else 5
                        |> string
                        |> str
                    ]
                ]
            ]
        ]
        hr []
        h4 [] [
            str "If this is your first time playing, it would be useful to read about "
            showModalLinkByName "Attributes and Skills" "attributes and skills" dispatch
            str ", "
            showModalLinkByName "Rolling" "rolling" dispatch
            str ", and "
            showModalLinkByName "Injuries" "injuries" dispatch
        ]
        button [OnClick (fun _ -> dispatch StartGame)] [str "I am ready to begin my tale..."]
    ]