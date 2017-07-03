module Wanderer.Data

module R = Fable.Helpers.React

type Continuation = {
    Description : Fable.Import.React.ReactElement
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
            Text = "Welcome to this \"bad\"venture!"
            Continuations = [{ Description = R.str "Go east"; NextPageName = "end2" }]
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