module Wanderer.Utils

open Fable.Core.JS

let toJson = JSON.stringify
let ofJson<'a> x = JSON.parse(x) :?> 'a