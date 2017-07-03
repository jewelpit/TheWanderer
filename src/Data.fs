module Wanderer.Data

type Page = {
    Text : string
}

let pages =
    [("start", { Text = "Welcome to this \"bad\"venture!" })]
    |> Map.ofList