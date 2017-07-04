module Wanderer.Modal

open Wanderer.Modals
open Wanderer.Model
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let showModalLink modal text dispatch =
    R.a [Nowhere; P.OnClick (fun _ -> dispatch <| ShowModal modal)] [
        R.str text
    ]

let showModalLinkByName modalName text dispatch =
    match Map.tryFind modalName Modals.modals with
    | None ->
        printfn "Could not find %s. Modals: %A" modalName modals
        R.str <| sprintf "**Could not find modal %s**" modalName
    | Some modal ->
        showModalLink modal text dispatch

let view modal innerElements dispatch =
    R.div [] [
        innerElements
        R.div [P.ClassName "modalBackground"] [
            R.div [P.ClassName "modal"] [
                R.h1 [P.ClassName "modalTitle"] [
                    R.str modal.Title
                    R.span [P.ClassName "close"; P.OnClick (fun _ -> dispatch CloseModal)] [R.str "×"]
                ]
                modal.Content
            ]
        ]
    ]
