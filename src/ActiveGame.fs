module Wanderer.ActiveGame

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.Data
open Wanderer.Model
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let view (character, page) dispatch =
    R.div [P.Style [P.Display "flex"]] [
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
                R.tr [] [R.td [P.ColSpan 2.; P.Style [P.Height "1em"]] []]
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
        R.div [P.ClassName "verticalDivider"] []
        R.div [P.ClassName "storyArea"] [
            R.p [] [R.str page.Text]
            R.ul [] [
                for cont in page.Continuations do
                    yield R.li [] [
                        cont.Description
                        R.br []
                        R.button [P.OnClick (fun _ -> dispatch (Flip cont.NextPageName))] [R.str "Choose"]]
            ]
        ]
    ]