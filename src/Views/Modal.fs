module Wanderer.Modal

open Wanderer.Model

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let view (title, contents) innerElements dispatch =
    R.div [] [
        innerElements
        R.div [P.ClassName "modalBackground"] [
            R.div [P.ClassName "modal"] [
                R.h1 [P.ClassName "modalTitle"] [
                    R.str title
                    R.span [P.ClassName "close"; P.OnClick (fun _ -> dispatch CloseModal)] [R.str "×"]
                ]
                contents
            ]
        ]
    ]
