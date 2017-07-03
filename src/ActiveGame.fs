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
    R.div [] [
        R.p [] [R.str page.Text]
        R.ul [] [
            for cont in page.Continuations do
                yield R.li [] [
                    cont.Description
                    R.br []
                    R.button [P.OnClick (fun _ -> dispatch (Flip cont.NextPageName))] [R.str "Choose"]]
        ]
    ]