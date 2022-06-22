module Wanderer.ViewHelpers

open Browser.Types
open Fable.Core
open Fable.React
open Fable.React.Props

/// Href that goes to nowhere (for custom clickable text).
let Nowhere = Href "javascript:;"

/// Wraps a string in a <p> tag.
let para text =
    p [] [str text]

/// Gets the value of the selected option in a select spinner.
[<Emit("$0.options[$0.selectedIndex].value")>]
let getSelectedValue (srcElement : EventTarget) : string = jsNative

/// Creates an option tag with the specified value and text.
let makeValueOption value text selected =
    option [Value <| value; Selected selected] [str text]