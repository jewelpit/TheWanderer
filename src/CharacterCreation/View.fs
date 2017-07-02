module Wanderer.CharacterCreation.View

open Elmish
open Fable.Core.JsInterop

open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let view dispatch =
    R.p [] [
        R.h1 [] [R.str "Character Creation"]
        para """
            So you wish to know the full story, stranger?  Well, as long as the coin for my drinks keeps coming out
            of your purse, I will keep talking.
            """
        R.p [] [
            R.str """
                To other humans, my name is Pompeia.  To your kind, I am known only as wanderer.  I have traveled far over
                these lands, and rarely come up against a problem I could not
                """
            R.select [] [
                R.option [] [R.str "think"]
                R.option [] [R.str "fight"]
            ]
            R.str "my way through.  When confronted with an obstacle, my first response is always to"
            R.select [] [
                R.option [] [R.str "talk my way out"]
                R.option [] [R.str "reach for my sword and gun"]
                R.option [] [R.str "prepare a spell"]
                R.option [] [R.str "sneak past"]
            ]
            R.str "."
        ]
    ]