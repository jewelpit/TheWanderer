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
    Text : string list
    Continuations : Continuation list
}

let pages =
    [
        {
            Name = "start"
            Text = ["""I first made my way into Tetznatalk as dusk was falling, after a long day of travel. There was an
                [[etzeznalt|Etzeznalt]] there."""]
            Continuations = [{ Description = R.str "Go east"; NextPageName = "middle" }]
        }
        {
            Name = "middle"
            Text = ["You made it closer..."]
            Continuations = [{ Description = R.str "Go easter"; NextPageName = "end" }]
        }
        {
            Name = "end"
            Text = ["You beat the game!"]
            Continuations = []
        }
    ]
    |> List.map (fun p -> (p.Name, p))
    |> Map.ofList

for kvp in pages do
    for continuation in kvp.Value.Continuations do
        if not (Map.containsKey continuation.NextPageName pages) then
            printfn "Room %A has an invalid contination: %s" kvp.Value continuation.NextPageName
    for part in List.collect Modals.parseLine kvp.Value.Text do
        match part with
        | Modals.Str _ -> ()
        | Modals.Link link ->
            if not (Map.containsKey link.LinkName Modals.modals) then
                printfn "Room %A has an invalid modal link: %s" kvp.Value link.LinkName