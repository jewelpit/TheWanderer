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
            Continuations = [{ Description = R.str "Go east"; NextPageName = "end" }]
        }
    ]
    |> List.map (fun p -> (p.Name, p))
    |> Map.ofList