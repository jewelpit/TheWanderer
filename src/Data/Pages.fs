module Wanderer.Pages

open Fable.Import.React

open Wanderer.Skills

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

type FailureEffect =
    | AlternatePage of string
    | AttributeDamage

type Condition =
    | Automatic
    | SkillCheckRequired of Attribute * Skill * int * FailureEffect
    | Bribe of int
    | Flags of string list * Condition

type Continuation = {
    Description : string
    NextPageName : string
    Condition : Condition
    SetFlags : string list
    GrantsMoney : int
}

type ContinuationBuilder() =
    member __.Build(description, nextPageName, ?condition, ?setFlags, ?grantsMoney) =
        {
            Description = description
            NextPageName = nextPageName
            Condition = defaultArg condition Automatic
            SetFlags = defaultArg setFlags []
            GrantsMoney = defaultArg grantsMoney 0
        }

type Page = {
    Name : string
    Text : string list
    Continuations : Continuation list
    Resets : bool
}

type PageBuilder() =
    member __.Build(name, text, continuations, ?resets) =
        {
            Name = name
            Text = text
            Continuations = continuations
            Resets = defaultArg resets false
        }

let private cb = ContinuationBuilder()
let private pb = PageBuilder()

let pages =
    [
        pb.Build(
            "start",
            [
                """I first made my way into Tetznatalk as dusk was falling, after a long day of travel.  I was
                weary, and looking forward to my first night indoors in nearly a month.  I rode up to the inn and
                stabled my [[monitor beetle|Monitor Beetle]], relieved that the inn seemed to have facilities to care
                for it."""
                """When I walked into the bar, I saw a shaky, bespectacled Etzen behind the counter, nervously
                cleaning a glass.  He was your typical rural farmer, dressed in the round black hat and brown kilt, and
                looked lost in thought.  At the sound of my arrival he looked up, and before he could stop blurted out,
                "Please!  You must help us!" """
            ],
            [cb.Build("\"Excuse me?\"", "tez1"); cb.Build("Enter test harness", "middle")])
        pb.Build(
            "tez1",
            [
                """He apologized, and told me that he was just desperate.  Lady Greltza, a local [[bandit|Bandit]] lord,
                had managed to trick the [[Town Guardian]] and steal her heartseed, without which she was slowly
                dying."""
                """ "Please," he said. "Please help us." """
            ],
            [
                cb.Build("Of course I agreed.  Bandits have done me wrong many times on my travels.", "tez2-approve")
                cb.Build("Of course I agreed.  I could never leave a town without its Town Guardian.", "tez2-approve")
                cb.Build(
                    "I turned them down.  \"Why should I help the people who scattered mine to the wind?\"",
                    "tez2-deny")
            ])
        pb.Build(
            "tez2-deny",
            [
                """He was shocked. "You doom us," he cried. But I stood resolute, and after a while he calmed down."""
                """ "Would you do it for 50 muld shells?" He asked eventually.  In this region, that sum would have
                allowed someone to live comfortably for two months."""
            ],
            [
                cb.Build(
                    "That amount of muld shells wouldn't be easy to come by otherwise, so I agreed.",
                    "tez2-approve",
                    grantsMoney=50)
                cb.Build(
                    """It's not unheard of for towns to survive long enough to get a new Town Guardian. I was
                        unconvinced.""",
                    "tez2-denytwice")
            ])
        pb.Build(
            "tez2-denytwice",
            ["\"Seventy shells!\""],
            [cb.Build("That seemed reasonable to me, so I accepted.", "tez2-approve", grantsMoney=70)])
        pb.Build(
            "tez2-approve",
            [
                """He thanked me profusely.  He went out and brought more villagers back while I prepared my room, and
                when I was done half of the town was in the first floor."""
                """They told me the story of how Greltza had used
                her [[Illusion Helm]] to take the form of a bumblebee, an animal beloved by the Guardian.  The Town
                Guardian was not able to distinguish Greltza from her other bumblebees, and Greltza was able to steal
                the heartseed.  Greltza fled town to the east, heading towards the [[Ereshkigal Mountains]]. If she
                continued on that path, she would eventually cross into the [[Szaltun Desert]], before finally arriving
                at [[Estaton, the Inland Sea|Estaton]]."""
                """We spoke and planned long into the night, but I was eventually able to get rest, and my steed and I
                were refreshed in the morning when we set out.  I regretted not meeting the Town Guardian, but no
                outsiders were allowed to visit her in her weakened state."""
            ],
            [cb.Build(
                """As the sun rose, I headed along the Great Eastern Road, towards the forested foothills of the
                    mountains...""",
                "eresh-1")]
        )


        pb.Build(
            "eresh-1",
            [
                """It was two days' journey along the [[Great Eastern Road]], making a good pace through the foothills
                before the mountains proper began, when I finally caught up with some of Greltza's scragglers.  It was
                early afternoon, right after I had finished my midday meal of dried pork strips that tasted like the
                saddle I was sitting in while I ate them.  I had to make good time to reduce their lead, and that meant
                no time to prepare food."""
            ],
            [
                cb.Build(
                    "Seeing them, I spurred my monitor beetle on, in an attempt to catch up to the bandits.",
                    "eresh-2",
                    SkillCheckRequired (Will, Sneaking, 1, AlternatePage "eresh-2-spotted"))
                cb.Build(
                    """Seeing them, I let my monitor beetle maintain its pace.  It wouldn't do to be noticed this early.,
                        and I figured I could always scout their positions at night.""",
                    "eresh-3")
            ])

        pb.Build(
            "eresh-2",
            [
                """I was able to ride closer without being spotted, and I was able to count five bandits in this group.
                They each had a circle pierced by a line, Greltza's symbol, painted on their saddlebags.  This group
                must have been delayed from the others."""
            ],
            [
                cb.Build(
                    "A chance like this might not come again, so I rode forward and shouted, \"Hoy!\"",
                    "eresh-2-persuasion",
                    SkillCheckRequired (Will, Persuasion, 2, AlternatePage "eresh-2-persuasion-failed"))
                cb.Build(
                    "A chance like this might not come again, so I rode up and prepared a sleeping spell.",
                    "eresh-2-sleeping",
                    SkillCheckRequired (Will, Ritual, 2, AttributeDamage))
                cb.Build(
                    """A chance like this might not come again, so I drew my sword, and spurred my monitor beetle into a
                        charge!""",
                    "eresh-2-fighting",
                    SkillCheckRequired (Might, Combat, 2, AttributeDamage))
                cb.Build(
                    """Having gotten a good look, I pulled back again.  I had learned their faces, and could now take my
                        time to plan.""",
                    "eresh-3")
            ])
        pb.Build(
            "eresh-2-spotted",
            [
                """Just my luck, the bastards had spotted me!  The five of them wheeled their monitor beetles around to
                face me.  I could see by the skulls on their belts that the four Etzen in the group were
                [[barbarians|Barbarians of Lagamut]].  To my surprise, one among the group was human!  It is not
                unheard of, for wanderers to join bandit crews, but it came as a shock nonetheless."""
            ],
            [
                cb.Build(
                    "Even the most unruly barbarians are still people.  I knew I could still talk my way out of this.",
                    "eresh-2-persuasion",
                    SkillCheckRequired (Will, Persuasion, 3, AlternatePage "eresh-2-persuasion-failed"))
                cb.Build(
                    """Money changes all minds, and I knew that if I gave them a small trinket they wanted, they would
                        let me pass in peace.""",
                    "eresh-2-bribed",
                    Bribe 50)
                cb.Build(
                    "I threw together a sleep spell, hoping that I wasn't too rushed to make the correct signs.",
                    "eresh-2-sleeping",
                    SkillCheckRequired (Will, Ritual, 3, AttributeDamage))
                cb.Build(
                    "If I was going to die, I was going to die on my mount, sword in my hand.",
                    "eresh-2-fighting",
                    SkillCheckRequired (Might, Combat, 3, AttributeDamage))
                cb.Build(
                    """At this point I'd seen enough.  I knew I had the fastest mount in the land, steered her around,
                        and retreated back the way I came.""",
                    "eresh-3",
                    setFlags=["ERESH-2-SPOTTED-RETREATED"])
            ])
        pb.Build(
            "eresh-2-persuasion",
            [
                """I offered some of my food to them, and spoke with them at length.  Over the course of our
                    conversation, I learned that three of them had grown up in a town that had lost its
                    [[Town Guardian]], and suffered terribly as a result.  They had had second thoughts about Greltza's
                    mission, and after I agreed to share from my wineskin they had convinced the other two that I should
                    be allowed to pass unhindered."""
            ],
            [cb.Build("With that, I continued on my way.", "eresh-4")])
        pb.Build(
            "eresh-2-persuasion-failed",
            [
                """The five of them were clearly under orders to allow no prisoners.  They drew their weapons, and made
                it clear that I would be spending the rest of time in a shallow grave on the side of the road, if
                scavenging animals didn't dig up my body first."""
            ],
            [
                cb.Build(
                    "I threw together a sleep spell, hoping that I wasn't too rushed to make the correct signs.",
                    "eresh-2-sleeping",
                    SkillCheckRequired (Will, Ritual, 3, AttributeDamage))
                cb.Build(
                    "If I was going to die, I was going to die on my mount, sword in my hand.",
                    "eresh-2-fighting",
                    SkillCheckRequired (Might, Combat, 3, AttributeDamage))
            ]
        )
        pb.Build(
            "eresh-2-sleeping",
            [
                """As I finished making the required hand gestures, the five bandits slumped forward in their saddles.
                    I made sure to tie their horses to nearby trees, so they wouldn't bolt and injure their riders."""
            ],
            [
                cb.Build("With the bandits taken care of, I headed forward along the road.", "eresh-4")
                cb.Build("Of course, that didn't stop me from going through their pockets.", "eresh-4", grantsMoney=20)
            ])
        pb.Build(
            "eresh-2-fighting",
            [
                """The fighting was long and difficult.  Not only was I outnumbered, they were all
                    [[barbarians|Barbarians of Lagamut]].  Never have there been more dangerous fighters."""
                """Between sword and spell, I was eventually able to push them back, and ultimately force them to
                    surrender.  It would have been much more effective to be able to take them back to the law, but for
                    now I had to content myself with binding them to trees and continuing on my way."""
            ],
            [
                cb.Build("I continued on.", "eresh-4")
                cb.Build(
                    "Turnabout is fair play, so I made sure to free them from their belongings before leaving.",
                    "eresh-4",
                    grantsMoney=20)
            ])
        pb.Build(
            "eresh-2-bribed",
            [
                """The five bandits let me pass, contentedly counting their muld shells.  With that many shells, I had
                    bought not only passage, but a promise they would not follow me for another hour."""
            ],
            [cb.Build("I continued on.", "eresh-4")])


        pb.Build(
            "middle",
            ["You made it closer..."],
            [cb.Build("Go easter", "end", SkillCheckRequired (Will, Persuasion, 30, AlternatePage "middle2"))])
        pb.Build(
            "middle2",
            ["You made it closer, but then you fucked up."],
            [
                cb.Build("Go eastest", "middle2", SkillCheckRequired (Might, Combat, 20, AttributeDamage), ["YouFuckedItUpViolently"])
                cb.Build("Go eastest, but pay", "middle2", Bribe 15, ["YouFuckedItUpPayedly"])
                cb.Build("Beat the game quickly", "middle3", Flags (["~YouFuckedItUpViolently"; "~YouFuckedItUpPayedly"], Automatic))
                cb.Build("Beat the game because you paid", "middle3", Flags (["~YouFuckedItUpViolently"; "YouFuckedItUpPayedly"], Automatic))
                cb.Build("Beat the game because you fought", "middle3", Flags (["YouFuckedItUpViolently"; "~YouFuckedItUpPayedly"], Automatic))
                cb.Build("Beat the game already!", "middle3", Flags (["YouFuckedItUpViolently"; "YouFuckedItUpPayedly"], Automatic))
            ])
        pb.Build("middle3", ["This'll reset you."], [cb.Build("Fine, just end it!", "end")], resets=true)
        pb.Build("end", ["You beat the game!"], [])
    ]
    |> List.map (fun p -> (p.Name, p))
    |> Map.ofList

let allRoutedPages =
    pages
    |> Seq.map (fun kvp -> kvp.Value)
    |> Seq.collect (fun page -> page.Continuations)
    |> Seq.collect (fun cont ->
        let rec getConditionPage cond =
            match cond with
            | Automatic -> [cont.NextPageName]
            | SkillCheckRequired (_, _, _, effect) ->
                match effect with
                | AlternatePage page -> [page]
                | AttributeDamage -> []
            | Flags (flags, c) -> getConditionPage c
            | _ -> []
        cont.NextPageName :: (getConditionPage cont.Condition))
    |> Set.ofSeq

let allSetFlags =
    pages
    |> Seq.map (fun kvp -> kvp.Value)
    |> Seq.collect (fun page -> page.Continuations)
    |> Seq.collect (fun cont -> cont.SetFlags)
    |> Set.ofSeq

let allReadFlags =
    pages
    |> Seq.map (fun kvp -> kvp.Value)
    |> Seq.collect (fun page -> page.Continuations)
    |> Seq.collect (fun cont ->
        let rec getConditionFlag cond =
            match cond with
            | Flags (flags, nextCond) -> flags @ (getConditionFlag nextCond)
            | _ -> []
        getConditionFlag cont.Condition)
    |> Seq.map (fun flag -> if flag.StartsWith("~") then flag.Substring(1) else flag)
    |> Set.ofSeq

let unreadFlags = Set.difference allSetFlags allReadFlags
if not (Set.isEmpty unreadFlags) then
    printfn "The following flags are set but never read: %A" unreadFlags

let unwrittenFlags = Set.difference allReadFlags allSetFlags
if not (Set.isEmpty unwrittenFlags) then
    printfn "The following flags are read but never set: %A" unwrittenFlags

for kvp in pages do
    if not (Set.contains kvp.Value.Name allRoutedPages) && kvp.Value.Name <> "start" then
        printfn "Page %s is not reachable from any other pages" kvp.Value.Name
    for continuation in kvp.Value.Continuations do
        if not (Map.containsKey continuation.NextPageName pages) then
            printfn "Page %s has an invalid continuation: %s" kvp.Value.Name continuation.NextPageName
        match continuation.Condition with
        | SkillCheckRequired (attr, skill, target, effect) ->
            match effect with
            | AlternatePage name ->
                if not (Map.containsKey name pages) then
                    printfn "Page %s has continuation with an invalid alternate page: %s." kvp.Value.Name name
            | AttributeDamage -> ()
        | _ -> ()
    for part in List.collect Modals.parseLine kvp.Value.Text do
        match part with
        | Modals.Str _ -> ()
        | Modals.Link link ->
            if not (Map.containsKey link.LinkName Modals.modals) then
                printfn "Page %s has an invalid modal link: %s" kvp.Value.Name link.LinkName