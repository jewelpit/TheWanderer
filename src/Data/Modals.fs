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

let modalLinkRegex = Regex(@"(\[\[.+?\]\])", RegexOptions.Compiled)

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
            Title = "Etzen"
            Content =
                R.div [] [
                    para """
                        Six to seven foot tall, owl-like, and the dominant species of this planet.  Although their
                        vestigial wing-flaps on their arms are no longer large enough for them to fly, they are
                        frequently used in local dances, ceremonies, or even just to soak up some heat on a nice day.
                        """
                    para """
                        Each Etzen village is guarded by a Town Guardian.  Town Guardians live in treehouses on the
                        outskirts of town, and draw their magical energy from the Heartseed in the center of their
                        trees.
                        """
                    para """
                        When humans crash landed on this planet hundreds of years ago, the Etzen, fearing them,
                        ordered that humans be scattered and forced to live as wanderers.
                        """
                ]
        }
        {
            Title = "Humans"
            Content =
                R.div [] [
                    para """
                        Humans, my people, crash landed on this planet nearly four hundred years ago.  We would not have
                        survived our first winter here were it not for the Etzens who discovered us, but a few short
                        years after they ensured our survival they announced that the humans would be scattered to the
                        winds, and forced to live as wanderers.  The Etzens had no idea what humans could do if
                        given a foothold on a planet, and were scared of the possibilities.
                        """
                    para """
                        Unlike Etzens, who have to recharge their magical power at Heartseeds, humans can pull magic
                        from the wider world around them.  This makes humans ideal travelers, and is a great aid to them
                        on their travels.
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
        {
            Title = "Monitor Beetle"
            Content =
                R.div [] [
                    para """
                        Monitor beetles are large, horned, wingless beetles that have been domesticated as livestock and
                        riding animals.
                        """
                ]
        }
        {
            Title = "Bandit"
            Content =
                R.div [] [
                    para """
                        The west coast area of this region was recently the forefront of a land war with the neighbor to
                        the northwest, but when the war ended, the mercenary soldiers stayed around as bandits, preying
                        on travelers and towns alike.
                        """
                    para "The worst bandits rise to the rank of bandit lord, and hold sway over several bandit tribes."
                ]
        }
        {
            Title = "Town Guardian"
            Content =
                R.div [] [
                    para """
                        Each Etzen town is guarded by a Town Guardian, a specially chosen and trained Etzen who receives
                        magical energy from the town's heartseed.  The Town Guardian is responsible for the physical
                        defense of the town, and is a position held for life, although when a Town Guardian is 45 years
                        old they may take on an apprentice.
                        """
                ]
        }
        {
            Title = "Illusion Helm"
            Content =
                R.div [] [
                    para """
                        The Illusion Helm is an ancient artifact, created by the deific Etzen of old.  When worn, the
                        Illusion Helm will allow the wearer to change their form to that of any animal they've seen.
                        Lady Greltza is the last known possessor of the helm.
                        """
                ]
        }
        {
            Title = "Ereshkigal Mountains"
            Content =
                R.div [] [
                    para """
                        The Ereshkigal mountains are a chain of volcanic mountains separating the western coastlands
                        from the Szaltun Desert to the east.  While Blight Pass is the safest route across the
                        Ereshkigal mountains, the Mines of Misan are the quickest.
                        """
                ]
        }
        {
            Title = "Szaltun Desert"
            Content =
                R.div [] [
                    para """
                        Also called The Desert of Ten Thousand Dunes, the Szaltun Desert lies to the east of the
                        Ereshkigal mountains.  Despite its initial appearance as a desolate wasteland, the Szaltun
                        Desert is actually host to a rich ecosystem of cacti, reptiles, and insects.
                        """
                ]
        }
        {
            Title = "Estaton"
            Content =
                R.div [] [
                    para """
                        Ah, Estaton: the Inland Sea.  Estaton is said to be the physical containment of the goddess
                        Estaton, chief among the deities.  It is only fitting that the strongest deity should go to the
                        greatest lake.
                        """
                ]
        }
        {
            Title = "Great Eastern Road"
            Content =
                R.div [] [
                    para """
                        The Great Eastern Road connects the coastal regions along the western coast to the desert beyond
                        the mountains.  No one can remember who first built the Great Eastern Road, and sections of it
                        have been mentioned in tales that are thousands of years old.
                        """
                    para """
                        At many points in the past, a strong nation has put the Great Eastern Road under strict control,
                        and it was safe and well traveled.  In these times, it is crawling with bandits, and only the
                        foolhardy travel it.
                        """
                ]
        }
        {
            Title = "Barbarians of Lagamut"
            Content =
                R.div [] [
                    para """
                        Lagamut, an island far across the ocean, is known far and wide for its varied collection of
                        venemous, poisonous, or otherwise deadly wildlife.  In this environment, Etzen and humans have
                        had to become stronger and more vicious in order to survive.  Each barbarian is born into a
                        crew, which are usually made up of around two hundred people, although membership in a crew is
                        not necessarily for life.
                        """
                ]
        }
        {
            Title = "Electrogun"
            Content =
                R.div [] [
                    para """
                        The electrogun is one of the most fearsome weapons created by the blind monks of Sizzhar.  By
                        reducing the electrical resistance of the air between the gun and the target the electrogun is
                        capable of transferring enormous amounts of electricity in an instant, raising the temperature
                        of the are it hits by thousands of degrees.
                        """
                ]
        }
    ]
    |> List.map (fun modal -> (modal.Title, modal))
    |> Map.ofList