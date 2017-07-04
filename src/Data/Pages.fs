module Wanderer.Pages

open Fable.Import.React

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

type Continuation = {
    Description : ReactElement
    NextPageName : string
}

type Page = {
    Name : string
    Text : string
    Continuations : Continuation list
}

let pages =
    [
        {
            Name = "start"
            Text = """I first made my way into Tetznatalk as dusk was falling, after a long day of travel. There was an
                [[etzeznalt|Etzeznalt]] there."""
            Continuations = [{ Description = R.str "Go east"; NextPageName = "end" }]
        }
        {
            Name = "end"
            Text = "You beat the game!"
            Continuations = []
        }
    ]
    |> List.map (fun p -> (p.Name, p))
    |> Map.ofList

for kvp in pages do
    for continuation in kvp.Value.Continuations do
        if not (Map.containsKey continuation.NextPageName pages) then
            printfn "Room %A has an invalid contination: %s" kvp.Value continuation.NextPageName