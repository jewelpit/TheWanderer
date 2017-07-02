module Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

/// Wraps a string in a <p> tag.
let para text =
    R.p [] [R.str text]