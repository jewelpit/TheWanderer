module Wanderer.Data

open Fable.Import.React

open Wanderer.Modals
open Wanderer.Model

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let pages =
    [
        {
            Name = "start"
            Text =
                fun dispatch state ->
                   R.div [] [
                       R.str "Welcome to this \"bad\"venture! There is a "
                       Rearkip dispatch
                       R.str " behind you!"
                   ]
            Continuations = [{ Description = R.str "Go east"; NextPageName = "end" }]
        }
        {
            Name = "end"
            Text = fun dispatch state ->
                    R.div [] [
                        R.str "You beat the game!"
                        R.h1 [] [R.str "COOOOOOOOL"]
                        R.button [
                            P.OnClick (fun _ -> dispatch (ShowModal ("this is my modal title", R.str "This is my modal text")))
                        ] [
                            R.str "click me!"
                        ]
                    ]
            Continuations = []
        }
    ]
    |> List.map (fun p -> (p.Name, p))
    |> Map.ofList

for kvp in pages do
    for continuation in kvp.Value.Continuations do
        if not (Map.containsKey continuation.NextPageName pages) then
            printfn "Room %A has an invalid contination: %s" kvp.Value continuation.NextPageName