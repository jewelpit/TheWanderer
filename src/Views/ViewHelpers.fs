module Wanderer.ViewHelpers

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

/// Href that goes to nowhere (for custom clickable text).
let Nowhere = P.Href "javascript:;"

/// Wraps a string in a <p> tag.
let para text =
    R.p [] [R.str text]

/// Gets the value of the selected option in a select spinner.
[<Emit("$0.options[$0.selectedIndex].value")>]
let getSelectedValue (srcElement : Element) : string = jsNative

/// Creates an option tag with the specified value and text.
let makeValueOption value text selected =
    R.option [P.Value <| U2.Case1 value; P.Selected selected] [R.str text]