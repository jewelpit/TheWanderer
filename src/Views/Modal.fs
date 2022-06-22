module Wanderer.Modal

open Wanderer.Modals
open Wanderer.Model
open Wanderer.ViewHelpers

open Fable.React
open Fable.React.Props

let showModalLink modal text dispatch =
    a [Nowhere; OnClick (fun _ -> dispatch <| ShowModal modal)] [
        str text
    ]

let showModalLinkByName modalName text dispatch =
    match Map.tryFind modalName Modals.modals with
    | None ->
        printfn "Could not find %s. Modals: %A" modalName modals
        str <| sprintf "**Could not find modal %s**" modalName
    | Some modal ->
        showModalLink modal text dispatch

let formatLine (text : string) dispatch =
    parseLine text
    |> List.map (fun part ->
        match part with
        | Str s -> str s
        | Link link -> showModalLinkByName link.LinkName link.DisplayName dispatch)
    |> div []

let view modal innerElements dispatch =
    div [] [
        innerElements
        div [ClassName "modalBackground"] [
            div [ClassName "modal"] [
                h1 [ClassName "modalTitle"] [
                    str modal.Title
                    span [ClassName "close"; OnClick (fun _ -> dispatch CloseModal)] [str "×"]
                ]
                modal.Content
            ]
        ]
    ]
