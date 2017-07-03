module Wanderer.CharacterCreation

open Elmish
open Fable.Core.JsInterop

open Wanderer.Model
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

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
                    | 0 -> { character with Persuasion = 4 }
                    | 1 -> { character with Combat = 4 }
                    | 2 -> { character with Ritual = 4 }
                    | 3 -> { character with Sneaking = 4 }
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
                    let idx : int = unbox e.nativeEvent.srcElement?selectedIndex
                    match idx with
                    | 0 -> { character with Persuasion = 2 }
                    | 1 -> { character with Combat = 2 }
                    | 2 -> { character with Ritual = 2 }
                    | 3 -> { character with Sneaking = 2 }
                    | i ->
                        printfn "Could not recognive index %d" i
                        character
                    |> Model.UpdateCharacter
                    |> dispatch)]
                [
                    R.option [] [R.str "talking to people"]
                    R.option [] [R.str "fighting"]
                    R.option [] [R.str "using magic"]
                    R.option [] [R.str "athletics"]
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
                    R.td [] [R.str <| string character.Persuasion]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Combat"]
                    R.td [] [R.str <| string character.Combat]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Ritual"]
                    R.td [] [R.str <| string character.Ritual]
                ]
                R.tr [] [
                    R.th [P.Style [P.TextAlign "right"]] [R.str "Sneaking"]
                    R.td [] [R.str <| string character.Sneaking]
                ]
            ]
        ]
    ]