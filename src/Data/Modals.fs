module Wanderer.Modals

open System.Text.RegularExpressions

open Fable.Import.React

open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

type Modal = {
    Title : string
    Content : ReactElement
}

type ModalLink = {
    LinkName : string
    DisplayName : string
}

type ParsedLine =
    | Str of string
    | Link of ModalLink

let modalLinkRegex = Regex(@"(\[\[\S+\]\])", RegexOptions.Compiled)

let parseLine (text : string) =
    let parts = modalLinkRegex.Split(text)
    let resultParts =
        parts
        |> Array.map (fun part ->
            let m = modalLinkRegex.Match(part)
            if m.Success then
                let matchPart = m.Groups.[1].Value
                let linkParts = matchPart.Substring(2, matchPart.Length - 4).Split([|'|'|])
                let displayName = Array.head linkParts
                let linkName = linkParts.[linkParts.Length - 1]
                Link { LinkName = linkName; DisplayName = displayName }
            else
                Str part)
        |> List.ofArray
    resultParts

let getDisplayLine (text : string) =
    parseLine text
    |> List.map (fun part ->
        match part with
        | Str s -> s
        | Link link -> link.DisplayName)
    |> List.reduce (+)

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
        {
            Title = "Attributes and Skills"
            Content =
                R.div [] [
                    para """
                        Pompeia has two core attributes: Might and Will.  Might determines how good Pompeia is at
                        physical tasks, and is most frequently paired with the Combat and Sneaking skills.  Will
                        determines how good Pompeia is at mental tasks, and is most frequently paired with the
                        Persuasion and Ritual skills.
                        """
                    para """
                        Pompeia's four skills are Persuasion, Combat, Ritual, and Sneaking.  When a skill roll is
                        required to advance the story, the button to select it will specify which attribute and skill
                        will be rolled.
                        """
                    para """
                        Muld refers to the shells of the tiny muld snail, which can be used to create a vibrant purple
                        dye. In rural areas, their sturdiness and portability have made them a de facto currency.
                        """
                ]
        }
        {
            Title = "Rolling"
            Content =
                R.div [] [
                    para """
                        When a story choice button says "Attempt," and lists an attribute, skill, and target number,
                        that means that a roll is required.  When you attempt a roll, the game will roll a number of
                        six-sided dice equal to your skill value (so if you have Combat 3, you would roll 3 dice on
                        Combat rolls).  Any of these dice whose value is equal to or less than the corresponding stat
                        counts as a success.  If at least as many successes as the target number have been rolled, the
                        skill roll is passed.
                        """
                    para """
                        When a skill roll is failed, one of two things will happen.  Either the story will fork, and you
                        will have to deal with the consequences of the failure, or the story will continue on the path
                        you chose and Pompeia will take an injury.
                        """
                ]
        }
        {
            Title = "Injuries"
            Content =
                R.div [] [
                    para """
                        When Pompeia fails skill rolls, she will either be sent on an alternate path or suffer an
                        injury.  Injuries come in two varieties: wounds and stress.  Wounds come from failing Might
                        tests and stress comes from failing Will tests.  When Pompeia suffers a number of wounds equal
                        to her Might score, she can no longer attempt Might tests.  When she suffers a number of wounds
                        equal to her Will score, she can no longer attempt Will tests.
                        """
                    para """
                        Injuries and stress are removed during special story events in game.
                        """
                ]
        }
    ]
    |> List.map (fun modal -> (modal.Title, modal))
    |> Map.ofList