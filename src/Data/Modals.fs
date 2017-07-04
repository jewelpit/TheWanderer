module Wanderer.Modals

open Fable.Import.React

open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

type Modal = {
    Title : string
    Content : ReactElement
}

let modals =
    [
        {
            Title = "Etzeznalt"
            Content =
                R.div [] [
                    para """
                        Etzeznalts the four to five foot tall, batlike, dominant species of this planet.  Although their
                        vestigial wings are no longer large enough for them to fly, they are frequently used in local dances,
                        ceremonies, or even just to soak up some heat on a nice day.
                        """
                    para """
                        Each Etzeznalt village is guarded by a Town Guardian.  Town Guardians live in treehouses on the
                        outskirts of town, and draw their magical energy from the Heartseed in the center of their trees.
                        """
                ]
        }
        {
            Title = "Humans"
            Content =
                R.div [] [
                    para """
                        Humans are what I am.  My people crash landed on this planet nearly four hundred years ago, but when we
                        asked the etzeznalt for refuge, we were instead scattered to the winds.  Forever after, etzeznalts have
                        known humans only as wanderers.
                        """
                    para """
                        Unlike Etzeznalts, who have to recharge their magical power at Heartseeds, humans can pull magic from
                        the wider world around them.  This makes humans ideal travelers, and is a great aid to them on their
                        travels.
                        """
                ]
        }
    ]
    |> List.map (fun modal -> (modal.Title, modal))
    |> Map.ofList