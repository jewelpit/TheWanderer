module Wanderer.Modals

open Wanderer.Model
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let rearkip text dispatch =
    R.a [Nowhere; P.OnClick (fun _ -> dispatch <| ShowModal ("Rearkip", (R.str "Rearkip are bat people.")))] [
        R.str text
    ]
let Rearkip = rearkip "Rearkip"