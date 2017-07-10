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
                """I first made my way into [[Tetznatalk]] as dusk was falling, after a long day of travel.  I was
                weary, and looking forward to my first night indoors in nearly a month.  I rode up to the inn and
                stabled my [[monitor beetle|Monitor Beetle]], relieved that the inn seemed to have facilities to care
                for it."""
                """When I walked into the bar, I saw a shaky, bespectacled Etzen behind the counter, nervously
                cleaning a glass.  He was your typical rural farmer, dressed in the round black hat and brown kilt, and
                looked lost in thought.  At the sound of my arrival he looked up, and before he could stop blurted out,
                "Please!  You must help us!" """
            ],
            [
                cb.Build("\"Excuse me?\"", "tez1")
                cb.Build("Enter test harness", "middle"(*, Flags (["TESTHARNESSENABLED"], Automatic)*))
            ]
        )
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
            ]
        )
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
            ]
        )
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
                continued on that path, she would eventually cross into the [[Szalkut Desert]], before finally arriving
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
                saddle I was sitting in.  I had to make good time to reduce their lead, and that meant no time to
                stop for luxuries like hot food."""
            ],
            [
                cb.Build(
                    "Seeing them, I spurred my monitor beetle on, in an attempt to catch up to the bandits.",
                    "eresh-2",
                    SkillCheckRequired (Will, Sneaking, 2, AlternatePage "eresh-2-spotted"))
                cb.Build(
                    """Seeing them, I let my monitor beetle maintain its pace.  It wouldn't do to be noticed this early,
                        and I figured I could always scout their positions at night.""",
                    "eresh-3")
            ]
        )

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
            ]
        )
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
                    "eresh-3-spotted_in_eresh_2")
            ]
        )
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
            ]
        )
        pb.Build(
            "eresh-2-fighting",
            [
                """The fighting was long and difficult.  Not only was I outnumbered, they were all
                    [[barbarians|Barbarians of Lagamut]].  Never have there been more dangerous fighters.  Two of their
                    number were also armed with [[electroguns|Electrogun]], whose brilliant arcs would have taken my head clean off my
                    shoulders if I let my concentration lapse for even a moment."""
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
            ]
        )
        pb.Build(
            "eresh-2-bribed",
            [
                """The five bandits let me pass, contentedly counting their muld shells.  With that many shells, I had
                    bought not only passage, but a promise they would not follow me for another hour."""
            ],
            [cb.Build("I continued on.", "eresh-4")])

        pb.Build(
            "eresh-3",
            [
                """I followed them from a distance for several hours, until the shadows grew and the world took on a red
                    hue.  When I saw them begin to settle down for the evening, I dismounted my beetle and made my way
                    forward through the underbrush."""
            ],
            [cb.Build(
                """I knew I would have to overtake them in order to catch up to Greltza. I led my beetle around them
                    under cover of darkness, careful not to make even the slightest noise.  After a time, I was able to
                    return to the road.""",
                "eresh-4",
                SkillCheckRequired (Might, Sneaking, 2, AlternatePage "eresh-3-spotted"))])
        pb.Build(
            "eresh-3-spotted_in_eresh_2",
            [
                """I was able to escape, and when I turned around to resume following them the bandits were on high
                    alert.  It was nothing but sheer luck that they didn't blow a horn, or radio Greltza.  As I followed
                    them throughout the day, they regularly looked back to keep track of how far I was behind them."""
                """Eventually they had to sleep, and when they set down for the night I knew my chance was here.  I rode
                    my [[beetle|Monitor Beetle] up to the edge of the undergrowth, dismounted, and plunged in."""
            ],
            [cb.Build(
                """I led my beetle through the underbrush, giving their camp a wide berth.""",
                "eresh-4",
                SkillCheckRequired (Might, Sneaking, 3, AlternatePage "eresh-3-spotted"))])
        pb.Build(
            "eresh-3-spotted",
            [
                """Damn!  It was just my luck to be spotted while I was tied up in the underbrush.  With my legs tangled
                    in shrubbery and my monitor beetle unharnessed, I was a sitting duck.  Matters grew worse when the
                    bright arc of an [[electrogun|Electrogun]] fired above my head.  This would be a desperate struggle.
                    """
            ],
            [
                cb.Build(
                    """I raised my hands. "Please," I said to them.  "I am just a simple traveler, who ran from
                        fright." """,
                    "eresh-3-social",
                    SkillCheckRequired (Will, Persuasion, 3, AlternatePage "eresh-3-social-failed"))
                cb.Build(
                    """I drew my sword.  They had numbers and range on me, but I was quick and my dark clothes blended
                        with the trees.""",
                    "eresh-3-fighting",
                    SkillCheckRequired (Might, Combat, 4, AttributeDamage))
                cb.Build(
                    """I began tracing magical forms with my arm.  A flashbang spell in this darkness could leave them
                        blinded for hours.""",
                    "eresh-3-magic",
                    SkillCheckRequired (Will, Ritual, 3, AttributeDamage))
            ]
        )
        pb.Build(
            "eresh-3-social",
            [
                """Thankfully, they believed my lie."""
            ],
            [cb.Build(
                "Unfortunately, pretending to be a scared traveler near bandits is not a cheap lie to make.",
                "eresh-4",
                Bribe 50)])
        pb.Build(
            "eresh-3-social-failed",
            [
                """ "You expect us to believe that?" the leader asked. "You're clearly comfortable with a sword, and you
                    have the eyes of someone who has killed before.  Now why don't you tell us why you're following us?"
                    """
                """ "Like hell I will," I replied."""
            ],
            [
                cb.Build(
                    "I dashed forward, sword flashing.",
                    "eresh-3-fighting",
                    SkillCheckRequired (Might, Combat, 4, AttributeDamage))
                cb.Build(
                    "I hastily cast a flashbang spell, blinding them.",
                    "eresh-3-magic",
                    SkillCheckRequired (Will, Ritual, 3, AttributeDamage))
            ]
        )
        pb.Build(
            "eresh-3-fighting",
            [
                """Thankfully, being ambushed in the bushes had meant that the bandits couldn't bring their
                    [[monitor beetles|Monitor Beetle]] to the fight, so they were just as slowed as I was in the
                    undergrowth.  I was able to leap from tree trunk to rock to avoid the [[electrogun|Electrogun]]
                    arcs, until I was close enough to disarm the human and take his gun."""
                """ "Listen up, you!" I shouted.  "Throw down your weapons or die." """

            ],
            [
                cb.Build("""They complied.""", "eresh-4")
                cb.Build(
                    """They complied, and I made sure their purses were the lighter for it.""",
                    "eresh-4",
                    grantsMoney=20)
            ]
        )
        pb.Build(
            "eresh-3-magic",
            [
                """With a flash brighter than the sun and a sound louder than any I had heard before, the five bandits
                    dropped to the ground, clutching their heads.  That would teach them to underestimate the people
                    they rob."""
            ],
            [
                cb.Build("I continued on my way.", "eresh-4")
            ]
        )

        pb.Build(
            "eresh-4",
            [
                """With the bandits no longer an issue, I decided that it was time for a small rest.  After some more
                    cold pork and a few hours' sleep under a drooping tree, I continued on as the sun broke over the
                    mountain ridge to the east."""
            ],
            [
                cb.Build("It was two more days until the next obstacle in my path.", "eresh-5")
            ]
        )

        pb.Build(
            "eresh-5",
            [
                """When I finally arrived at the great granite gates to the [[Lost Mines]], I was relieved to see that
                    it looked like a group of people had been camping here recently.  Greltza!  She had been here only
                    hours before!"""
                """I looked up at the doors, towering twenty feet above me.  It is said that in the old days a password
                    was needed to gain entry to this door, but the hinges were time-worn and the doors were slightly off
                    their hinges, with just enough room to squeeze a monitor beetle and rider through."""
                """I clicked my beetle forward, and she deftly made her way around the broken stonework.  The air in
                    there was surprisingly fresh, a testament to the multitude of known and unknown entrances to the
                    mines.  I reached around in my saddlebag and pulled out a small [[glowbulb|Glowbulb]] that I could
                    strap to my head.  I would need both hands free in here."""
                """After an hour or two, I came to a fork in the path where I could no longer track the bandits'
                    footprints through the dust on the ground.  I sat there for a time, astride my beetle, before making
                    my decision."""
            ],
            [
                cb.Build(
                    "I clicked my beetle forwards, going up the stairs to my left.",
                    "eresh-6",
                    setFlags=["HIGH_ROAD"])
                cb.Build(
                    "I clicked my beetle forwards, going down the stairs to my right.",
                    "eresh-7")
            ]
        )

        pb.Build(
            "eresh-6",
            [
                """I continued along the high path the rest of the day, before eventually having to make camp.  I
                    allowed myself the luxury of a fire, made from wood I had stored in my saddlebags just in case.
                    I knew that the slight, constant breeze would get rid of the smoke, and being in this enclosed
                    tunnel would prevent any bandits from seeing my light.  Earlier that day I had killed a rabbit
                    with a spell as it crossed the trail, and I enjoyed my first hot meal in days."""
                """It's funny now, but at the time, this lonely dinner, two hundred feet underground with no one but
                    myself and my beetle, was the most content I would be on this journey."""
            ],
            [
                cb.Build(
                    """I woke many hours later, unsure what time it was.  I put my glowbulb back on my head,
                        saddled my monitor beetle, and made my way further down the tunnel.""",
                    "eresh-8")
            ]
        )

        pb.Build(
            "eresh-7",
            [
                """The stairs were long, and it took my beetle nearly twenty minutes to wend down them.  This part of
                    the mines was dug near the end of their operation, and the tunnels here were smaller, less regular,
                    and more unfinished.  Progress was slow, since I had to periodically steer my beetle around old
                    mining equipment and other debris."""
                """It was at this point that I heard voices up ahead.  They sounded like [[Deskites]]: lanky, blind,
                    pale skinned people who live in caves.  Deskites are usually content to do their own thing and let
                    travelers pass on their way, but the [[Lost Mines]] are anything but usual."""
            ],
            [
                cb.Build("I decided that the direct route was best.  I rode straight up to the Deskites.", "eresh-9")
                cb.Build(
                    "I decided that it was best to avoid a direct encounter, so I snuck by them.",
                    "eresh-9-sneaking",
                    SkillCheckRequired (Might, Sneaking, 2, AlternatePage "eresh-9-spotted"))
            ]
        )

        pb.Build(
            "eresh-8",
            [
                """After I had traveled for a while, not sure how long, I heard a faint sound up ahead.  As I got
                    closer, I realized that the sound was someone crying.  I was familiar with bandits using someone
                    pretending to be in distress as bait for an ambush, so I slowed my beetle and approached cautiously.
                    """
                """When I got closer, I saw that the source of the sound was a human man, with a neatly trimmed beard
                    and the appearance of a merchant wanderer.  He was quietly sobbing, surrounded by shredded and
                    shattered goods.  In the corner, I saw the curled up corpse of a monitor beetle.  It had once been
                    covered with rich decorative paint, but much of it was burnt off by [[electrogun|Electrogun]] fire.
                    """
                """Upon my approach the man looked up.  I stil remember the look of his eyes when he just choked out
                    one word: "Please." """
                """I was able to find out that his name was Flavian, and he had been making his way from the
                    [[Szalkut Desert]] to the coast when he ran head-on into Greltza's band.  They stole all of his
                    easily portable goods and destroyed the rest, then bound him while they made camp and ate the legs
                    and head of his beetle.  Apparently they had left only an hour ago.  I was getting close!"""
            ],
            [
                cb.Build(
                    """What had happened to this man was a tragedy, but I had no time to lose.  I gave him a few bites
                        of food and went on my way.""",
                    "eresh-8-b")
                cb.Build(
                    """What had happened to this man was a tragedy, but I had no time to lose.  I gave him a few muld
                        shells and went on my way.""",
                    "eresh-8-b",
                    Bribe 20,
                    setFlags=["PAID_FLAVIAN"])
            ]
        )
        pb.Build(
            "eresh-8-b",
            [
                """At this point the path forward grew treacherous.  My [[glowbulb|Glowbulb]] began to flicker, even
                    though I had recharged in the sun only two days ago.  Shadows grew, and I noticed that the floor of
                    the tunnel had accumulated a large amount of loose debris.  Irregular holes sprouted up in the walls
                    and floors, no doubt dug by the mines' unofficial new residents."""
            ],
            [
                cb.Build(
                    "I advanced forward with caution.  I can't make up time if my monitor beetle broke a leg.",
                    "eresh-10",
                    SkillCheckRequired (Will, Sneaking, 4, AttributeDamage))
            ]
        )

        pb.Build(
            "eresh-9",
            [
                """I walked my beetle up to the [[Deskites]].  When I was able to get close enough to get a good look,
                    I saw that they were all huddled together, and seemed to be crying.  I was downwind of them, and I
                    could smell the pungent nutmeg-scent they give off when agitated.  One of them turned to face me
                    when I came close enough that my beetle's footfalls could be felt through the ground."""
                """ "Leave us, uplander!" she said in the characteristic Deskite drawl.  "We have suffered enough this
                    day." """
                """With a little gentle prodding, I was able to find out that Greltza and her crew had come through this
                    passage, and taken the child that this hexad was raising.  The breeding pair in the hexad had been
                    having trouble conceiving, and when little Altexen was finally born it was hailed as a small
                    miracle. Now, to have the child stolen from them..."""
            ],
            [
                cb.Build(
                    "Greltza will pay.  I will return your child to you.", "eresh-9-climb", setFlags=["DESKITE_CHILD"])
            ]
        )
        pb.Build(
            "eresh-9-sneaking",
            [
                """Sneaking past the [[Deskites]] was easier than expected.  I was able to hug the wall on the opposite
                    end of the tunnel from the alcove where they were staying, and therefore stay out of the range where
                    they could feel my footfalls through the floor.  By the time I was upwind of them I would have
                    enough of a lead that I could easily ride away."""
                """On my way past, though, I noticed something interesting.  The Deskites appeared to be huddled
                    together, in some kind of mourning."""
            ],
            [
                cb.Build(
                    "In my travels I had known Deskites to be friendly, so I decided to ask them what was wrong.",
                    "eresh-9-sneaking-talking")
                cb.Build(
                    """However, every minute that Tetznatalk went without its town guardian, it was closer to
                        destruction.  I pressed on my way.""",
                    "eresh-9-climb")
            ]
        )
        pb.Build(
            "eresh-9-spotted",
            [
                """It's one of the great rules of the world that you should never try to sneak past someone in a narrow
                    tunnel.  It's even more true if the people you're trying to sneak past can feel the vibrations from
                    your footfalls.  I had barely made it within fifty feet of them before they turned to face me."""
            ],
            [
                cb.Build(
                    """I had always known Deskites to be friendly, so I decided to stop and talk.  Perhaps they had seen
                        Greltza.""",
                    "eresh-9-sneaking-talking")
                cb.Build(
                    """I had a gap that I could shoot through if my beetle was fast enough. I dug in my heels and we
                        sprinted past them.""",
                    "eresh-9-climb")
            ]
        )
        pb.Build(
            "eresh-9-sneaking-talking",
            [
                """ "Leave us, uplander!" the closest Deskite said in the characteristic Deskite drawl.  "We have
                    suffered enough this day." """
                """With a little gentle prodding, I was able to find out that Greltza and her crew had indeed come
                    through this passage, and taken the child that this hexad was raising.  The breeding pair in the
                    hexad had been having trouble conceiving, and when little Altexen was finally born it was hailed as
                    a small miracle. Now, to have the child stolen from them..."""
            ],
            [
                cb.Build(
                    "Greltza will pay.  I will return your child to you.", "eresh-9-climb", setFlags=["DESKITE_CHILD"])
            ]
        )
        pb.Build(
            "eresh-9-climb",
            [
                """At this point the tunnel bent sharply upward, so sharply that it became almost more of a climb than
                    a walk.  My monitor beetle's feet began to slip, and I knew that I would have to dismount and let
                    it walk unencumbered."""
                """As my beetle and I struggled up the steep passageway, I thought about Greltza.  Why would she steal
                    the heartseed?  They have no power to anyone except a [[Town Guardian]], and there's no market for
                    them.  What reason would Greltza have to steal it other than cruelty?"""
                """I was so lost in thought that I almost didn't realize it when the passageway leveled out again.  I
                    looked around.  I must have rejoined the upper path."""
            ],
            [
                cb.Build("After taking a moment to rest, I pressed on.", "eresh-10-child")
            ]
        )

        pb.Build(
            "eresh-10",
            [
                """I arrived at a bridge over a great chasm.  On the other end, a large set of doors sat, slightly open.
                    In front of the doors there was a large landing, maybe sixty feet or so on a side.  And there, in
                    the middle of the landing, my quarry: Greltza and her bandits."""
                """Greltza looked much as I expected.  Her brilliant grey plumage was dusty from the mines, and she was
                    dressed for action, wearing only a sleeveless tunic, baggy traveling pants, and her
                    [[Illusion Helm]].  At her hip she had both a short sword and a [[sun pistol|Sun Pistol]].  Beneath
                    her helm, I could see that she had a sharpened steel beak cover, which looked like it could deliver
                    terrible, ripping wounds when the fighting was close and desperate."""
                """Unfortunately, Greltza's senses were better than I expected.  I only had a scant few seconds to
                    observe her and her coterie of bandits before she looked directly at me, and paused.  Then, she
                    waved.  She turned to a burly human who looked to be her lieutenant, spoke some words in his ear,
                    then disappeared through the door."""
                """Her lieutenant, in turn, spoke with a few of the bandits that were left, then followed Greltza
                    through the doors, taking all of the bandits with him except for four."""
            ],
            [
                cb.Build(
                    """Did they really underestimate me so?  I made it past five of their number not but a few days ago.
                        I drew my sword and kicked my beetle into a charge!""",
                    "eresh-10-fighting",
                    SkillCheckRequired (Might, Combat, 4, AlternatePage "eresh-10-fighting-failed"))
                cb.Build(
                    """I had no time to waste.  Greltza was right there!  I rode forward, preparing a concussive blast
                        spell.  I could blow them aside without even having to stop my beetle.""",
                    "eresh-10-magic",
                    SkillCheckRequired (Will, Ritual, 4, AlternatePage "eresh-10-magic-failed"))
                cb.Build(
                    """I knew that in many bandit crews, the leader ruled only through fear.  With Greltza so close I
                        would have to pay more, but I knew they could still be bribed.""",
                    "eresh-10-bribed",
                    Bribe 75)
            ]
        )
        pb.Build(
            "eresh-10-child",
            [
                """I arrived at a bridge over a great chasm.  On the other end, a large set of doors sat, slightly open.
                    In front of the doors there was a large landing, maybe sixty feet or so on a side.  And there, in
                    the middle of the landing, my quarry: Greltza and her bandits."""
                """Greltza looked much as I expected.  Her brilliant grey plumage was dusty from the mines, and she was
                    dressed for action, wearing only a sleeveless tunic, baggy traveling pants, and her
                    [[Illusion Helm]].  At her hip she had both a short sword and a [[sun pistol|Sun Pistol]].  Beneath
                    her helm, I could see that she had a sharpened steel beak cover, which looked like it could deliver
                    terrible, ripping wounds when the fighting was close and desperate."""
                """My heart sank as I saw the Deskite child, hands bound behind their back, being led on a rope out the
                    door.  There was no chance I was letting Greltza escape now."""
                """Unfortunately, Greltza's senses were better than I expected.  I only had a scant few seconds to
                    observe her and her coterie of bandits before she looked directly at me, and paused.  Then, she
                    waved.  She turned to a burly human who looked to be her lieutenant, spoke some words in his ear,
                    then disappeared through the door."""
                """Her lieutenant, in turn, spoke with a few of the bandits that were left, then followed Greltza
                    through the doors, taking all of the bandits with him except for four."""
            ],
            [
                cb.Build(
                    """Did they really underestimate me so?  I made it past five of their number not but a few days ago.
                        I drew my sword and kicked my beetle into a charge!""",
                    "eresh-10-fighting",
                    SkillCheckRequired (Might, Combat, 4, AlternatePage "eresh-10-fighting-failed"))
                cb.Build(
                    """I had no time to waste.  Greltza was right there!  I rode forward, preparing a concussive blast
                        spell.  I could blow them aside without even having to stop my beetle.""",
                    "eresh-10-magic",
                    SkillCheckRequired (Will, Ritual, 4, AlternatePage "eresh-10-magic-failed"))
                cb.Build(
                    """I knew that in many bandit crews, the leader ruled only through fear.  With Greltza so close I
                        would have to pay more, but I knew they could still be bribed.""",
                    "eresh-10-bribed",
                    Bribe 75)
            ]
        )
        pb.Build(
            "eresh-10-fighting",
            [
                """They were no match for me.  My sword flashed in the sliver of sunlight coming in through the doors,
                    and after a few minutes they realized their predicament and ran through the doors into the desert
                    beyond."""
            ],
            [
                cb.Build(
                    """I kicked my beetle, and she ran through the doors.  There wasn't a chance that Greltza could
                        escape from me this day!""",
                    "szalk-1")
            ]
        )
        pb.Build(
            "eresh-10-fighting-failed",
            [
                """I realized that these bandits were a fair bit more experienced than the ones I had bested earlier.  I
                    was pressed into a corner, and realized that if I didn't do something, and soon, my beetle and I
                    would be lying broken at the bottom of this chasm, pushed over the edge."""
            ],
            [
                cb.Build(
                    """I yanked on the reigns, and my beetle reared up.  I took advantage of the confusion to kick her
                        forward, and rode past the bandits.  If I could make it to the door...""",
                    "szalk-1-bandits")
            ]
        )
        pb.Build(
            "eresh-10-magic",
            [
                """They were no match for me.  As I finished my spell a wave of force shot out and slammed them into the
                    walls.  """
            ],
            [
                cb.Build(
                    """I kicked my beetle, and she ran through the doors.  There wasn't a chance that Greltza could
                        escape from me this day!""",
                    "szalk-1")
            ]
        )
        pb.Build(
            "eresh-10-magic-failed",
            [
                """Unknown to me, one of the bandits had a [[noisestick|Noisestick]], and its concussive burst knocked
                    me seneseless before I could finish my spell.  Thankfully my beetle was not affected, and it was
                    able to run through the door, past the bandits, with me on it."""
            ],
            [
                cb.Build(
                    "I made it through the door, slightly dazed and with the bandits in hot pursuit.",
                    "szalk-1-bandits")
            ]
        )
        pb.Build(
            "eresh-10-bribed",
            [
                """When I finished counting out the shells, the bandits went back the way they came, deeper into the
                    mines.  Perhaps in a few months I would be called back to deal with these four.  In the meanwhile,
                    though, I had more important work."""
            ],
            [
                cb.Build("I rode my beetle through the open door.", "szalk-1")
            ]
        )


        pb.Build(
            "szalk-1",
            [
                """After two days underground, the light of the sun was blinding.  I reached into my saddlebags and
                    retrieved my tinted riding goggles.  With my eyes protected from the sun and the sand, I could now
                    see the full vastness of the [[Szalkut Desert]] stretching before me, with sand and cacti all that I
                    could make out.  About a mile away, I saw a large plume of sand, and a large object moving quicly.
                    Greltza!  She had a sand sled with eight monitor beetles waiting at the exit, and was making her way
                    across the desert with her full crew."""
                """There was still time.  My beetle was far less encumbered than theirs, and their sled could only move
                    at the speed of its slowest beetle."""
            ],
            [
                cb.Build("I kicked my beetle forward into a sprint and gave chase.", "szalk-2")
            ]
        )
        pb.Build(
            "szalk-1-bandits",
            [
                """I galloped out the door, thankful that these four bandits were on foot while I was mounted.  The sun
                    was blinding after two days underground, and I had to ride almost blind for several minutes before
                    I felt safe to slow down enough to pull out my tinted riding goggles from my saddlebag."""
                """I surveyed the horizon.  How could Greltza have gotten so far so quickly?  The answer came to me when
                    I noticed a plume of dust about two miles away.  I couldn't quite tell, but it looked like she had a
                    sand sled waiting at the exit of the mines, and was now riding away on that with her crew."""
            ],
            [
                cb.Build("I kicked my beetle forward into a sprint and gave chase.", "szalk-2-delayed")
            ]
        )

        pb.Build(
            "szalk-2",
            [
                """It took ten minutes, but I caught up with Greltza and her sled.  The sand it was kicking up from
                    behind was too thick to ride in, so I rode up alongisde the sled on the left.  From this close I
                    could see Greltza standing, arms crossed, eyes straight ahead.  She bent her head down, and looked
                    directly into my eyes.  Then she turned her head forward again, and raised her hand."""
                """On signal, her lieutenant walked up to the edge of the sled, set his foot upon the wall, and
                    unholstered his own [[sun pistol|Sun Pistol]].  Unless I acted quickly, my quest would take a turn
                    for the dead."""
            ],
            [
                cb.Build(
                    """I leapt off my monitor beetle, grabbing onto some ropes on the side of the giant sled.  Hand over
                        hand, I climbed to the railing, jumped over, and drew my sword.  I was going to take care of
                        this lieutenant.""",
                    "szalk-2-combat",
                    SkillCheckRequired (Might, Combat, 3, AlternatePage "szalk-2-fallen"))
                cb.Build(
                    """I leapt off my monitor beetle, grabbing onto some ropes on the side of the giant sled.  Hand over
                        hand, I climbed to the railing, jumped over, and drew my sword.  I was going to rescue this
                        child.""",
                    "szalk-2-combat-child",
                    Flags (["DESKITE_CHILD"], SkillCheckRequired (Might, Combat, 3, AlternatePage "szalk-2-fallen")))
                cb.Build(
                    """I didn't have time to make my way up to the sled to fight.  I began a body-lock spell, hoping I
                        could get it cast before the lieutenant finished charging his pistol.""",
                    "szalk-2-magic",
                    SkillCheckRequired (Will, Ritual, 3, AlternatePage "szalk-2-magic-failed"))
                cb.Build(
                    """ "Please!  Return the child!  They've done nothing to you!" I shouted. """,
                    "szalk-2-persuasion-child",
                    Flags (["DESKITE_CHILD"], SkillCheckRequired (Will, Persuasion, 3, AlternatePage "szalk-2-persuasion-failed")))
            ]
        )
        pb.Build(
            "szalk-2-delayed",
            [
                """It took nearly an hour, but I caught up with Greltza and her sled.  The sand it was kicking up from
                    behind was too thick to ride in, so I rode up alongisde the sled on the left.  From this close I
                    could see Greltza standing, arms crossed, eyes straight ahead.  She bent her head down, and looked
                    directly into my eyes.  Then she turned her head forward again, and raised her hand."""
                """On signal, her lieutenant walked up to the edge of the sled, set his foot upon the wall, and
                    unholstered his own [[sun pistol|Sun Pistol]].  Unless I acted quickly, my quest would take a turn
                    for the dead."""
            ],
            [
                cb.Build(
                    """I leapt off my monitor beetle, grabbing onto some ropes on the side of the giant sled.  Hand over
                        hand, I climbed to the railing, leapt over, and drew my sword.  I was going to take care of
                        this lieutenant.""",
                    "szalk-2-combat",
                    SkillCheckRequired (Might, Combat, 4, AlternatePage "szalk-2-fallen"))
                cb.Build(
                    """I leapt off my monitor beetle, grabbing onto some ropes on the side of the giant sled.  Hand over
                        hand, I climbed to the railing, leapt over, and drew my sword.  I was going to rescue this
                        child.""",
                    "szalk-2-combat-child",
                    Flags (["DESKITE_CHILD"], SkillCheckRequired (Might, Combat, 4, AlternatePage "szalk-2-fallen")))
                cb.Build(
                    """I didn't have time to make my way up to the sled to fight.  I began a body-lock spell, hoping I
                        could get it cast before the lieutenant finished charging his pistol.""",
                    "szalk-2-magic",
                    SkillCheckRequired (Will, Ritual, 4, AlternatePage "szalk-2-magic-failed"))
                cb.Build(
                    """ "Please!  Return the child!  They've done nothing to you!" I shouted. """,
                    "szalk-2-persuasion-child",
                    Flags (["DESKITE_CHILD"], SkillCheckRequired (Will, Persuasion, 4, AlternatePage "szalk-2-persuasion-failed")))
            ]
        )
        pb.Build(
            "szalk-2-combat",
            [
                """His shot with the [[sun pistol|Sun Pistol]] missed me by inches.  I ducked down and kicked his legs
                    out from under him.  He fell sideways, and his head landed near my hand.  This was my chance!
                    Reaching back, I pulled out my knife and stuck it to his throat while I held my sword up to keep the
                    other bandits at bay."""
                "\"One wrong move, and he dies,\" I said."
                """The bandits paused their advance and looked to their leader.  Greltza uncrossed her arms and rested
                    her hands on her hips."""
                "\"We outnumber you.  If he dies, you die a second later.\""
                "We seemed to be at an impasse."
            ],
            [
                cb.Build("I'll return him if you return the heartseed.", "szalk-2-sled-crash")
                cb.Build(
                    "I'll return him if you return the child.",
                    "szalk-2-sled-crash",
                    Flags (["DESKITE_CHILD"], Automatic))
            ]
        )
        pb.Build(
            "szalk-2-combat-child",
            [
                """His shot with the [[sun pistol|Sun Pistol]] missed me by inches.  I ducked down and kicked his legs
                    out from under him.  He fell backwards, and I leapt to my feet.  I sprinted forward to the child,
                    making sure to give the fallen bandit lieutenant a good kick in the stomach as I did so."""
                """ "If you want the child back so badly, have them," Greltza said, as she pushed the child backwards
                    off the far end of the sled."""
                """I turned and ran back to the railing of the side I had climbed up, fighting off bandits with my
                    sword as I went.  I dropped down off the edge of the railing, back onto my faithful monitor beetle.
                    I'd have to catch up to Greltza later."""
            ],
            [
                cb.Build("I circled back around, searching for the child.", "szalk-2-child-retrieved")
            ]
        )
        pb.Build(
            "szalk-2-persuasion-child",
            [
                """Her lieutenant let his [[sun pistol|Sun Pistol]] finish charging, but waited for Greltza's signal
                    before firing.  Greltza looked down in thought for a few seconds."""
                "\"Sure,\" she said, as she shoved the child backwards off the far end of the sled."
                "I gasped in surprise, and reared my beetle back to slow down."
            ],
            [
                cb.Build("I circled back around, searching for the child.", "szalk-2-child-retrieved")
            ]
        )
        pb.Build(
            "szalk-2-magic",
            [
                """I completed my spell right as the lieutenant fired.  He fell backwards and his shot went wild,
                    missing me by several feet."""
                """Now that I had earned myself some breathing room, I clambered up the side of the slide."""
            ],
            [
                cb.Build("\"Greltza!\" I shouted. \"Return the heartseed!\"", "szalk-2-sled-crash")
                cb.Build(
                    "\"Greltza!\" I shouted. \"Return the child!\"",
                    "szalk-2-magic-child",
                    Flags (["DESKITE_CHILD"], Automatic))
            ]
        )
        pb.Build(
            "szalk-2-magic-failed",
            [
                """My attempt to cast a body-lock spell in time failed.  Right as the [[sun pistol|Sun Pistol]] finished
                    charging I had to drop my spell and pull back sharply on the reigns to avoid getting my head blown
                    off by the bolt.  My beetle reared, and a good thing too--not even a second later four electrogun
                    blasts hit the sand right where my beetle had been.  Greltza and her bandits would be on high alert
                    now."""
            ],
            [
                cb.Build(
                    "I would have to settle for following them from a distance, and attempting to ambush them later.",
                    "szalk-3")
            ]
        )
        pb.Build(
            "szalk-2-magic-child",
            [
                "\"Sure,\" she said, as she shoved the child backwards off the far end of the sled."
                "I gasped in surprise, and reared my beetle back to slow down."
            ],
            [
                cb.Build("I circled back around, searching for the child.", "szalk-2-child-retrieved")
            ]
        )
        pb.Build(
            "szalk-2-fallen",
            [
                """Right as I was nearing the top of the railing the sled jerked to the right, and my hand slipped.
                    With one hand suddenly not holding on, I involuntarily rolled to the side while desperately trying
                    to maintain purchase with my other hand.  This was the only thing that saved my life, as half a
                    second later a [[sun pistol|Sun Pistol]] bolt shot through where my head was a second before."""
                """I let myself drop back onto the saddle of my beetle.  As I pulled it back, four
                    [[electrogun|Electrogun]] blasts hit the sand where I was.  With them on this high alert, there was
                    no way I could try boarding again."""
            ],
            [
                cb.Build(
                    "I would have to settle for following them from a distance, and attempting to ambush them later.",
                    "szalk-3")
            ]
        )
        pb.Build(
            "szalk-2-child-retrieved",
            [
                """I circled back, and began my search.  After about twenty minutes, I found Altexen, the
                    [[Deskite|Deskites]] child hiding in a shallow valley where some cacti had grown between two dunes.
                    """
                "\"I am here to bring you back to your parental hexad,\" I said."
                "Altexen did not look convinced, for understandable reasons. \"Prove my parents sent you!\""
                """I told Altexen that I knew their name and the story of their parents' difficulties with conception.
                    This was enough to convince them that I was who I said I was.  Now came the difficult part.  I told
                    Altexen that while I understood that they wanted nothing more than to be reunited with their
                    parents, I had to bring the person who had captured them to justice, or else an entire town would
                    die.  Altexen was terrified about the prospect of being near Greltza again, but to their credit they
                    understood and agreed."""
            ],
            [
                cb.Build(
                    "Now it was time to catch up to Greltza, and finally bring her to justice.",
                    "szalk-3",
                    setFlags=["CHILD_RETRIEVED"])
            ]
        )
        pb.Build(
            "szalk-2-sled-crash",
            [
                """This is the moment where my memory becomes hazy.  I saw the bandit leading the sled turn around, and
                    I saw her yell something, and then my entire world flipped on its side at a gallop."""
                """I remember tumbling.  And sand.  And pain."""
                """I awoke later, partially buried by sand and a hundred feet from the sled, which was lying upside-down
                    and split into several pieces.  Greltza, her bandits, and their beetles were gone.  I whistled for
                    my beetle, and ten minutes saw it pop its head over a dune, coming at me at a run.  I let loose a
                    huge sigh of relief.  I'd have hated to be stuck in the middle of this desert without a mount."""
            ],
            [
                cb.Build(
                    """After making sure that myself and my beetle had no permanent injuries, I mounted and headed off
                        after the tracks Greltza and her crew had made.""",
                    "szalk-3-nosled",
                    Flags (["~DESKITE_CHILD"], Automatic))
                cb.Build(
                    """After making sure that myself and my beetle had no permanent injuries, I mounted and headed off
                        after the tracks Greltza and her crew had made.""",
                    "szalk-2-sled-crash-child-retrieved",
                    Flags (["DESKITE_CHILD"], Automatic))
            ]
        )
        pb.Build(
            "szalk-2-sled-crash-child-retrieved",
            [
                """I began my search.  After about twenty minutes, I found Altexen, the [[Deskite|Deskites]] child,
                    hiding in a shallow valley where some cacti had grown between two dunes."""
                "\"I am here to bring you back to your parental hexad,\" I said."
                "Altexen did not look convinced, for understandable reasons."
                """I told Altexen that I knew their name and the story of their parents' difficulties with conception.
                    This was enough to convince them that I was who I said I was.  Now came the difficult part.  I told
                    Altexen that while I understood that they wanted nothing more than to be reunited with their
                    parents, I had to bring the person who had captured them to justice, or else an entire town would
                    die.  Altexen was terrified about the prospect of being near Greltza again, but to their credit they
                    understood and agreed."""
            ],
            [
                cb.Build(
                    "Now it was time to catch up to Greltza, and finally bring her to justice.",
                    "szalk-3-nosled",
                    setFlags=["CHILD_RETRIEVED"])
            ]
        )

        pb.Build(
            "szalk-3",
            [
                """I had been following Greltza's sled for about two hours when I noticed that the horizon had begun to
                    look a little... smudgy.  Instead of being able to make out a clear definition of sand and sky, all
                    I could see along the horizon was a soft, diffuse brown.  As I continued following the sled, I
                    realized to my horror that we were riding directly into a sandstorm."""
            ],
            [
                cb.Build("With no choice, I followed Greltza.", "szalk-3-sandstorm")
            ]
        )
        pb.Build(
            "szalk-3-nosled",
            [
                """I had been following Greltza's band on their beetles for about two hours when I noticed that the
                    horizon had begun to look a little... smudgy.  Instead of being able to make out a clear definition
                    of sand and sky, all I could see along the horizon was a soft, diffuse brown.  As I continued
                    following the band, I realized to my horror that we were riding directly into a sandstorm."""
            ],
            [
                cb.Build("With no choice, I followed Greltza.", "szalk-3-sandstorm")
            ]
        )
        pb.Build(
            "szalk-3-sandstorm",
            [
                """Greltza's lieutenant moved his hands, and a magical dome surrounded Greltza and her crew.  As they
                    entered the sandstorm head on, I was forced to respect their courage.  Not many people would go head
                    on into a sandstorm, even with magical protection, and if her lieutenant's concentration wavered for
                    even a moment, the barrier would fall."""
                """Then again, I was riding in with no allies other than myself to fall back on if something went wrong,
                    so what does that say about me?"""
            ],
            [
                cb.Build(
                    "I put on my large traveling cloak and tightened my riding goggles.  This was going to be rough.",
                    "szalk-3-sandstorm-nomagic",
                    Flags (["~CHILD_RETRIEVED"], Automatic))
                cb.Build(
                    "I put on my large traveling cloak and tightened my riding goggles.  Fortunately for me, this type
                        of protection spell is one of the first I learned.",
                    "szalk-3-sandstorm-magic",
                    Flags (["~CHILD_RETRIEVED"], SkillCheckRequired (Will, Ritual, 2, AttributeDamage)))
                cb.Build(
                    """I put on my large traveling cloak and tightened my riding goggles, then tucked Altexen in the
                        front of my cloak.  This was going to be rough.""",
                    "szalk-3-sandstorm-nomagic",
                    Flags (["CHILD_RETRIEVED"], Automatic))
                cb.Build(
                    """I put on my large traveling cloak and tightened my riding gogglesthen tucked Altexen in the
                        front of my cloak.  Fortunately for me, this type of protection spell is one of the first I
                        learned.""",
                    "szalk-3-sandstorm-magic",
                    Flags (["CHILD_RETRIEVED"], SkillCheckRequired (Will, Ritual, 2, AttributeDamage)))
            ]
        )
        pb.Build(
            "szalk-3-sandstorm-nomagic",
            [
                """The sand was blinding, and the wind howled in my ears until I thought I would never hear another
                    sound again.  The air smelled of ozone, and there was a definite sense of charge, and unease.  Even
                    through my thick traveling cloak, my skin felt like it was beaten raw.  I do not know how long I was
                    in the sandstorm, but it felt like an eternity."""
            ],
            [
                cb.Build("Regardless, I pressed on.", "szalk-4-offtarget")
            ]
        )
        pb.Build(
            "szalk-3-sandstorm-magic",
            [
                """The sand was blinding, and the wind howled in my ears until I thought I would never hear another
                    sound again.  The air smelled of ozone, and there was a definite sense of charge, and unease.  If it
                    weren't for my magical shield, I do not know how I would have made it through.  I do not know how
                    long I was in the sandstorm, but it felt like an eternity."""
            ],
            [
                cb.Build("At long last, I passed the sandstorm behind.", "szalk-4")
            ]
        )

        pb.Build(
            "szalk-4",
            [
                """When I finally cleared the sandstorm, I wasn't surprised to see the sun nearly setting.  I made camp,
                    and when the stars came out I was happy to see that I was right on track.  Were I attempting to go
                    to Estaton, I was on exactly the route I would take."""
            ],
            [
                cb.Build(
                    "I went to bed, knowing that I would be able to sleep long enough to begin the morning rested.",
                    "szalk-4-morning",
                    Flags (["~CHILD_RETRIEVED"], Automatic))
                cb.Build(
                    """I went to bed, knowing that Altexen and I would be able to sleep long enough to begin the morning
                        rested.""",
                    "szalk-4-morning",
                    Flags (["CHILD_RETRIEVED"], Automatic))
            ]
        )
        pb.Build(
            "szalk-4-offtarget",
            [
                """When I finally cleared the sandstorm, I wasn't surprised to see the sun nearly setting.  I made camp,
                    but when the stars came out I looked at them and realized that I was far off course from where I was
                    supposed to be.  Were I attempting to go to Estaton, I would be nowhere near here: I would instead
                    try to be several hours' travel north.  Damn!"""
            ],
            [
                cb.Build(
                    "I went to bed, knowing that I would have to wake far earlier than I wanted in the morning.",
                    "szalk-4-morning",
                    Flags (["~CHILD_RETRIEVED"], Automatic),
                    setFlags=["TIRED"])
                cb.Build(
                    """I went to bed, knowing that Altexen and I would have to wake far earlier than I wanted in the
                        morning.""",
                    "szalk-4-morning",
                    Flags (["CHILD_RETRIEVED"], Automatic),
                    setFlags=["TIRED"])
            ]
        )
        pb.Build(
            "szalk-4-morning",
            [
                """I woke bright and early, and headed out.  I made it a point to check maprooms every time I stopped in
                    a town, and I prided myself on knowing the locations of all major cities and ports within hundreds
                    of miles.  If Greltza was running, I had a good idea where to."""
                """Around midday I figured I must have been going in more or less the right direction, because I saw
                    three people I recognized as being in Greltza's band up ahead.  I clicked my beetle forward, and
                    when the three bandits saw me they kicked their beetles and galloped off.  I vowed that I would not
                    let them easily escape."""
                """As I was chasing them, I saw them look to the right, and their faces turned white with fear.  They
                    kicked their beetles even harder.  Nervous, I checked what they were looking at, and saw an
                    [[Etzen]] riding the largest lizard I'd ever seen, almost twice the size of my monitor beetle, at
                    incredible speed.  This mysterious Etzen was gaining on us, easily."""
            ],
            [
                cb.Build("I rode my beetle as hard as I could, but I was still overtaken.", "szalk-4-noon")
            ]
        )
        pb.Build(
            "szalk-4-noon",
            [
                """ "Excuse me!" the mysterious Etzen shouted as they pulled up alongside me.  "Please excuse me if this
                    is a weird question, but would you happen to be a bandit?  Please answer me honestly, as I have a
                    magic ring that can tell me if you're lying." """
                """ "Of course not!" I shouted back.  At this point I noticed that the Etzen looked a little... off. 
                    Their feathers were patchy and grey, and they moved their body in an odd, jerky fashion.  "In fact,
                    I'm trying to bring these three in front of me to justice!  They've stolen the heartseed from a
                    [[Town Guardian]], and doomed the village of [[Tetznatalk]]!" """
                """ "That does seem unconscionable," they said.  "Would you like some help?" """
            ],
            [
                cb.Build("\"Of course I'd like some damn help!\" I replied.", "szalk-4-noon-2")
            ]
        )
        pb.Build(
            "szalk-4-noon-2",
            [
                """ "Very well," the Etzen said as they took out a whip.  They swung the whip around, and it grew longer
                    and longer.  When they finally whipped it forward, it was several hundred feet long, and wrapped
                    around one of the bandits, yanking him out of his saddle.  The Etzen did this for the two others,
                    and once they were without mounts we easily rode them down and bound their hands and feet."""
                """The bandits were shaking with fear. "It's the Desert Knight... we're doomed," one of them said.  That
                    piqued my interest."""
                "\"Desert Knight?\" I asked."
                "\"She--\" the bandit began."
                "\"They, please,\" asked Desert Knight."
                """ "They ride around the desert, killing people left and right!  They're a mummified menace, killed two
                    hundred years ago and then brought back to life by their own hatred of the living!" """
                """ "While it may be true that I am a two hundred year old mummy, I hardly go around the desert killing
                    for fun.  All I do is bring bandits to justice.  The rest is just unfounded rumors." """
                """Once we had finished with the introductions, Desert Knight bid me farewell and said they were
                    taking in these three to the nearest authorities."""
            ],
            [
                cb.Build(
                    "\"Wait!\" I said. \"What if I said I knew where their leader was heading?\"",
                    "szalk-4-noon-3",
                    Flags (["~TIRED"], Automatic))
                cb.Build(
                    "\"Wait!\" I said. \"What if I said I knew where their leader was heading?\"",
                    "szalk-4-noon-3",
                    Flags (["TIRED"], SkillCheckRequired (Will, Persuasion, 3, AttributeDamage)))
            ]
        )
        pb.Build(
            "szalk-4-noon-3",
            [
                """Desert Knight seemed interested.  I explained that it would be much more useful to the world as a
                    whole if they would help me capture Gretza.  They sighed, and admitted that I was right."""
                """ "We can't bring these three with us," they said."""
                """ "I know." """
                """ So, with that we let them go.  We gave them one of our waterskins, and directed them to the nearest
                    town."""
                """ "So, where to from here?" Desert Knight asked."""
            ],
            [
                cb.Build("\"We go the direst route, through the [[Lizard Fields]],\" I replied.", "szalk-5")
                cb.Build(
                    "\"We go the safe route, through [[Dahl Oasis]],\" I replied.",
                    "szalk-6",
                    Flags (["~HIGH_ROAD"], Automatic))
                cb.Build(
                    """ "I met a merchant several days ago who came from [[Dahl Oasis]]." I replied. "Maybe we'd be able
                        to find less legitimate merchants there who would know where bandits go to sell their goods."
                        """,
                    "szalk-6",
                    Flags (["HIGH_ROAD"], Automatic))
            ]
        )

        pb.Build(
            "szalk-5",
            [
                """One thing I should mention in my tale is that Desert Knight was quite talkative.  It turns out
                    that when you've been a mummy for 200 years with no one but bandits and sheriffs to ever talk to,
                    you end up with a lot of stuff that you want to talk about.  For my part, I was happy to oblige.
                    Desert Knight had done me a favor, and if that meant listening to them spend an hour talking
                    about their favorite rare variety of local cactus, then so be it.  And besides, you never know when
                    weird tidbits you know about cacti might save your life."""
                """It took us about four hours to make it to the [[Lizard Fields]], all told.  Once we got there, I
                    could see how it got its name.  Huge lizard skeletons, ones that put even Desert Knight's mount
                    to shame, lay scattered across the landscape.  I could see packs of [[bone hyenas|Bone Hyenas]],
                    terrifying hunting animals that eat bones and would prefer my fresh ones to the ancient ones on
                    display, wandering in and out of the skeletons.  If we were going to cross the [[Lizard Fields]],
                    we would have to make it past the hyenas."""
            ],
            [
                cb.Build(
                    "Bone hyenas generally have dull senses.  They shouldn't be too hard to sneak past.",
                    "szalk-5-sneaking",
                    SkillCheckRequired (Might, Sneaking, 2, AlternatePage "szalk-5-discovered"))
                cb.Build(
                    "Bone hyenas are not amazing fighters.  I drew my sword, daring any to come close.",
                    "szalk-5-fought",
                    SkillCheckRequired (Might, Combat, 2, AttributeDamage))
                cb.Build(
                    """Bone hyenas are generally cowardly, scavenging animals.  A fear spell should disperse them
                        nicely.""",
                    "szalk-5-magic",
                    SkillCheckRequired (Will, Ritual, 2, AttributeDamage))
            ]
        )
        pb.Build(
            "szalk-5-sneaking",
            [
                """Desert Knight and I were able to find a path along which we could guide our mounts in order to
                    avoid ever coming too close to a [[bone hyena|Bone Hyenas]].  After what seemed like forever, we
                    were through the [[Lizard Fields]]."""
            ],
            [
                cb.Build("Free of the Lizard Fields, we pressed on.", "szalk-5-end")
            ]
        )
        pb.Build(
            "szalk-5-discovered",
            [
                """Desert Knight and I led our mounts carefully, making sure to give the [[bone hyenas|Bone Hyenas]]
                    a wide berth.  What we didn't count on was a sudden change in the direction of the wind, which
                    immediately took us from comfortable downwind of the hyenas to directly upwind of them.  They
                    smelled us in an instant, and suddenly the pack went from a group of docile, sleeping predators into
                    a frenzy of activity as the hyenas practically ran over themselves to surround us."""
            ],
            [
                cb.Build(
                    "I drew my sword.  I would never be bested by a mere animal.",
                    "szalk-5-fought",
                    SkillCheckRequired (Might, Combat, 3, AttributeDamage))
                cb.Build(
                    "I prepared a fear spell.  Bone hyenas are naturally skittish creatures.",
                    "szalk-5-magic",
                    SkillCheckRequired (Will, Ritual, 3, AttributeDamage))
            ]
        )
        pb.Build(
            "szalk-5-fought",
            [
                """I fought against the pack of [[bone hyenas|Bone Hyenas]], carefully inflicting only light flesh
                    wounds.  When enough of the pack had been wounded, they wisely decided they had had enough and
                    turned tail and fled."""
            ],
            [
                cb.Build(
                    "With the bone hyenas out of the way, Desert Knight and I were able to continue on our way.",
                    "szalk-5-end")
            ]
        )
        pb.Build(
            "szalk-5-magic",
            [
                """As I completed the spell, a burst of darkness shot forth from my fingertips.  Whenever it touched a
                    [[bone hyena|Bone Hyenas]], the hyena would yelp as if in pain and sprint away as fast as it could.
                    In only a few short seconds, our path was cleared."""
            ],
            [
                cb.Build(
                    "With the bone hyenas out of the way, Desert Knight and I were able to continue on our way.",
                    "szalk-5-end")
            ]
        )
        pb.Build(
            "szalk-5-end",
            [
                """As we left the [[Lizard Fields]], I noticed tracks in the sand.  I got off my
                    [[beetle|Monitor Beetle]] to investigate, and when I was looking at the tracks I noticed something
                    glinting in the sand a few feet away.  I went over, and when I pulled the object out of the sand, I
                    realized to my excitement that it was a waterskin with the symbol of Greltza's bandits painted on
                    the side.  These were their tracks!"""
            ],
            [
                cb.Build("We followed the tracks, tight on Greltza's heels.", "szalk-7")
            ]
        )

        pb.Build(
            "szalk-6",
            [
                """Getting to [[Dahl Oasis]] took many hours, and it was nearly nightfall by the time we arrived there.
                    When we arrived, the day traders were packing up their tents, and the night traders were setting
                    theirs up.  As the premier trading outpost in the [[Szalkut Desert]], Dahl Oasis did not sleep."""
                """We checked into an inn, where those of us who still live could sleep, while Desert Knight spent
                    the night speaking with traders and gathering information."""
            ],
            [
                cb.Build(
                    """While I was talking to the innkeeper, a brown-feathered Etzen, about arranging payment, I asked
                        her if she had ever heard of a trader named Flavian.  When she said she had, I told her of how I
                        had run across him in the mines, and how I was seeking the one who had done this to him.""",
                    "szalk-6-vengeance",
                    Flags (["HIGH_ROAD"; "~PAID_FLAVIAN"], Automatic))
                cb.Build(
                    """While I was talking to the innkeeper, a brown-feathered Etzen, about arranging payment, I asked
                        her if she had ever heard of a trader named Flavian.  When she said she had, I told her of how I
                        had run across him in the mines, and how I was seeking the one who had done this to him.""",
                    "szalk-6-vengeance",
                    Flags (["HIGH_ROAD"; "PAID_FLAVIAN"], Automatic),
                    setFlags=["VENGEANCE_PLUS"])
                cb.Build(
                    """While I was talking to the innkeeper, a brown-feathered Etzen, about arranging payment, I asked
                        her if she knew anything about routes bandits tended to take through the desert.""",
                    "szalk-6-intel",
                    SkillCheckRequired (Will, Persuasion, 2, AttributeDamage))
                cb.Build(
                    """While I was talking to the innkeeper, a brown-feathered Etzen, about arranging payment, I asked
                        her if she knew anything about routes bandits tended to take through the desert.  I also made
                        sure to let her know I could make answering this worth her while.""",
                    "szalk-6-intel",
                    Bribe 20)
            ]
        )
        pb.Build(
            "szalk-6-vengeance",
            [
                """The innkeeper leaned in close.  "Everyone here knows Flavian.  He's well-loved in this town, not just
                    because of his skill at trading, but also because of his kindness and sensitivity.  If someone truly
                    did this to him, we will stop at nothing to help you hunt them down."""
                """ "My sister got mixed up with banditry when she was younger.  I remember her telling me that Shouzas,
                    one of the best fences on this side of the continent, has a shop on the shores of [[Estaton]] that
                    can ship stolen goods to any city-state along the borders of the lake, and sell goods repeatedly in
                    so many cities that nothing he sells can ever be tracked again." """
                """My heart leapt to hear this news.  I had a definite location!  I thanked the innkeeper graciously,
                    and after an hour's time exchanging stories and pleasantries I headed to bed."""
            ],
            [
                cb.Build(
                    """By this point it was late, so I went upstairs, tucked Altexen into the bed, and sang them a story
                        that my father had sung to me as a child, until they fell asleep.  Then I rolled my bedroll out
                        on the floor next to the bed, and passed into sleep.""",
                    "szalk-6-morning",
                    Flags (["CHILD_RETRIEVED"; "~VENGEANCE_PLUS"], Automatic))
                cb.Build(
                    """By this point it was late, so I went upstairs, got into bed, and thought about the days ahead of
                        me.  I knew that with how close I was to Greltza's destination, I would be facing her again in
                        two days.  I couldn't tell if the prospect scared me or excited me more.""",
                    "szalk-6-morning",
                    Flags (["~CHILD_RETRIEVED"; "~VENGEANCE_PLUS"], Automatic))
                cb.Build(
                    """The innkeeper also told me that the day guard Shouzas's port was an admirer of Flavian, and that
                        that knowledge might be useful to me.  I thanked her graciously.  It had gotten late by this
                        point, so I went upstairs, tucked Altexen into the bed, and sang them a story that my father had
                        sung to me as a child, until they fell asleep.  Then I rolled my bedroll out on the floor next
                        to the bed, and passed into sleep.""",
                    "szalk-6-morning",
                    Flags (["CHILD_RETRIEVED"; "VENGEANCE_PLUS"], Automatic))
                cb.Build(
                    """The innkeeper also told me that the day guard Shouzas's port was an admirer of Flavian, and that
                        that knowledge might be useful to me.  I thanked her graciously.  It had gotten late by this
                        point, so I went upstairs, got into bed, and thought about the days ahead of me.  I knew that
                        with how close I was to Greltza's destination, I would be facing her again in two days.  I
                        couldn't tell if the prospect scared me or excited me more.""",
                    "szalk-6-morning",
                    Flags (["~CHILD_RETRIEVED"; "VENGEANCE_PLUS"], Automatic))
            ]
        )
        pb.Build(
            "szalk-6-intel",
            [
                """ The innkeeper leaned in closer."""
                """ "My sister got mixed up with banditry when she was younger.  I remember her telling me that Shouzas,
                    one of the best fences on this side of the continent, has a shop on the shores of [[Estaton]] that
                    can ship stolen goods to any city-state along the borders of the lake, and do this so many times
                    that nothing he sells can ever be tracked again." """
                """My heart leapt to hear this news.  I had a definite location!  I thanked the innkeeper graciously,
                    and after an hour's time exchanging stories and pleasantries I headed to bed."""
            ],
            [
                cb.Build(
                    """I tucked Altexen into the bed, and sang them a story that my father had sung to me as a child
                        until they fell asleep.  Then I rolled my bedroll out on the floor next to the bed, and passed
                        into sleep.""",
                    "szalk-6-morning",
                    Flags (["CHILD_RETRIEVED"], Automatic))
                cb.Build(
                    """I got into bed, and thought about the days ahead of me.  I knew that with how close I was to
                        Greltza's destination, I would be facing her again in two days.  I couldn't tell if the prospect
                        scared me or excited me more.""",
                    "szalk-6-morning",
                    Flags (["~CHILD_RETRIEVED"], Automatic)
                )
            ]
        )
        pb.Build(
            "szalk-6-morning",
            [
                """Morning came, and Desert Knight was waiting patiently outside the room."""
                """ "I was able to find something out," they said.  "I learned that when a heartseed is stolen, it
                    physically binds to the person who has taken it, and is not removable for several days.  I did the
                    math, and there's no way that Greltza could sell the seed within the next two days.  We know exactly
                    how much time we have!" """
                """I let Desert Knight know what I had found out about Greltza's destination, and they were as excited
                    as I was that we seemed to have all the information we would need to plan our final assault.  It was
                    then I realized that I had never actually asked Desert Knight their name."""
                """ "Do you have a name that I could call you that's more personal than 'Desert Knight?'" I asked."""
                """ "Not really," Desert Knight replied. "I don't really remember anything from before my death, other
                    than my oath to protect the weak.  Any name I took would feel fake, because I don't really feel like
                    I'm truly the person before.  I feel like it's most correct if I have no name." """
            ],
            [
                cb.Build(
                    """With all that said, Desert Knight and I headed out.  I would bring back the heartseed and the
                        child.""",
                    "szalk-7",
                    Flags (["DESKITE_CHILD"; "~CHILD_RETRIEVED"], Automatic))
                cb.Build(
                    "With all that said, Desert Knight and I headed out.",
                    "szalk-7",
                    Flags (["~DESKITE_CHILD"; "~CHILD_RETRIEVED"], Automatic))
                cb.Build(
                    "With all that said, Desert Knight, Altexen, and I headed out.",
                    "szalk-7",
                    Flags (["CHILD_RETRIEVED"], Automatic))
            ]
        )

        pb.Build(
            "szalk-7",
            [
                """We crested a tall dune, and suddenly the entirety of [[Estaton]] was before us.  If I hadn't seen
                    maps that showed all of the land around this lake, I would have sworn I was on the ocean coast
                    again.  Thousands of rivers traveled thousands of miles to create this lake before me, and it was an
                    inspiring site."""
                """Desert Knight told me to look down, and I spotted a small trading port on the shore, with a stone
                    walls and a large compliment of guards."""
                "\"This must be where Greltza is going to fence the heartseed,\" I said."
                "Desert Knight nodded. \"Hm.\""
            ],
            [
                cb.Build(
                    """Time was of the essence.  Since there were undoubtedly people here who would recognize Desert
                        Knight, I rode up to the front gate alone.  Greltza and her bandits would still recognize me on
                        sight, but we were gambling that they weren't close enough with the boss here to have told the
                        guards too much about my appearance.""",
                    "szalk-7-persuasion",
                    Flags (
                        ["~CHILD_RETRIEVED"; "~VENGEANCE_PLUS"],
                        SkillCheckRequired (Will, Persuasion, 3, AlternatePage "szalk-7-persuasion-failed")))
                cb.Build(
                    """Time was of the essence.  Since there were undoubtedly people here who would recognize Desert
                        Knight, they guarded the Altexen behind a dune while I rode up to the front gate alone.  Greltza
                        and her bandits would still recognize me on sight, but we were gambling that they weren't close
                        enough with the boss here to have told the guards too much about my appearance.""",
                    "szalk-7-persuasion",
                    Flags (
                        ["CHILD_RETRIEVED"; "~VENGEANCE_PLUS"],
                        SkillCheckRequired (Will, Persuasion, 3, AlternatePage "szalk-7-persuasion-failed")))
                cb.Build(
                    """Time was of the essence.  Since there were undoubtedly people here who would recognize Desert
                        Knight, I rode up to the front gate alone.  Greltza and her bandits would still recognize me on
                        sight, but we were gambling that they weren't close enough with the boss here to have told the
                        guards too much about my appearance.""",
                    "szalk-7-persuasion-vengeance",
                    Flags (
                        ["~CHILD_RETRIEVED"; "VENGEANCE_PLUS"],
                        SkillCheckRequired (Will, Persuasion, 2, AlternatePage "szalk-7-persuasion-failed")))
                cb.Build(
                    """Time was of the essence.  Since there were undoubtedly people here who would recognize Desert
                        Knight, they guarded the Altexen behind a dune while I rode up to the front gate alone.  Greltza
                        and her bandits would still recognize me on sight, but we were gambling that they weren't close
                        enough with the boss here to have told the guards too much about my appearance.""",
                    "szalk-7-persuasion-vengeance",
                    Flags (
                        ["CHILD_RETRIEVED"; "VENGEANCE_PLUS"],
                        SkillCheckRequired (Will, Persuasion, 2, AlternatePage "szalk-7-persuasion-failed")))
                cb.Build(
                    """Time was of the essence.  Since there were undoubtedly people here who would recognize Desert
                        Knight, I rode up to the front gate alone.  Greltza and her bandits would still recognize me on
                        sight, but we were gambling that they weren't close enough with the boss here to have told the
                        guards too much about my appearance.""",
                    "szalk-7-bribe",
                    Flags (
                        ["~CHILD_RETRIEVED"],
                        Bribe 35))
                cb.Build(
                    """Time was of the essence.  Since there were undoubtedly people here who would recognize Desert
                        Knight, they guarded the Altexen behind a dune while I rode up to the front gate alone.  Greltza
                        and her bandits would still recognize me on sight, but we were gambling that they weren't close
                        enough with the boss here to have told the guards too much about my appearance.""",
                    "szalk-7-bribe",
                    Flags (
                        ["CHILD_RETRIEVED"],
                        Bribe 35))
                cb.Build(
                    """Time was of the essence.  Desert Knight and I rode around to the side wall.  The walls were bare
                        stone, and easy to climb.  When we got to the top, we slipped down the other side and entered
                        the town.""",
                    "szalk-7-sneaking",
                    SkillCheckRequired (Might, Sneaking, 3, AlternatePage "szalk-7-sneaking-failed"))
                cb.Build(
                    """Time was of the essence, but this was a well-defended facility.  We decided that it was better to
                        wait until nightfall.""",
                    "szalk-7-night")
            ]
        )
        pb.Build(
            "szalk-7-persuasion",
            [
                "\"State your business,\" the guard commanded."
                "\"I am a wanderer who wishes to trade wares.  Is that not what this place is for?\" I responded."
                """The guard looked suspicious of me, but I was able to produce some trinkets that looked valuable
                    enough that he eventually let me inside.  I was in."""
            ],
            [
                cb.Build(
                    """I wandered around the post, knowing that I would run into Greltza sooner rather than later.  I
                        just hoped that I saw her before she saw me.""",
                    "szalk-7-wandering"
                )
            ]
        )
        pb.Build(
            "szalk-7-persuasion-vengeance",
            [
                "\"State your business,\" the guard commanded."
                """I leaned in close.  "Are you familiar with Flavian, the merchant?" I asked the guard."""
                """ "Flavian's Gazetteer is my favorite series of books in the world!  I love reading about his travels,
                    and pretending that I'm there seeing the sites and meeting the people." """
                """Perfect, I thought.  I told the guard the entire horrible story about Greltza and Flavian.  By the
                    time I was done, the guard's facial feathers were raised, which Etzen do involuntary when furious.
                    """
                "\"If anyone asks, you climbed the wall,\" he said as he let me into the trading post."
            ],
            [
                cb.Build(
                    """I wandered around the post, knowing that I would run into Greltza sooner rather than later.  I
                        just hoped that I saw her before she saw me.""",
                    "szalk-7-wandering"
                )
            ]
        )
        pb.Build(
            "szalk-7-sneaking",
            [
                """Desert Knight and I looked around.  We seemed to have landed behind a public house, and we agreed
                    that since Desert Knight was probably very well know in this area, they would stay in the shadows
                    while I investigated."""
            ],
            [
                cb.Build(
                    """I wandered around the post, knowing that I would run into Greltza sooner rather than later.  I
                        just hoped that I saw her before she saw me.""",
                    "szalk-7-wandering",
                    setFlags=["DESERT_KNIGHT_BACKUP"]
                )
            ]
        )
        pb.Build(
            "szalk-7-bribe",
            [
                "\"State your business,\" the guard commanded."
                "\"I am a wanderer who wishes to trade wares.  Is that not what this place is for?\" I responded."
                """The guard looked suspicious of me, so I reached into my pocket and pulled out some muld shells."""
                """ "To prove I'm serious," I said as I dropped them into his palm.  He counted them, then waved me by.
                    I was in."""
            ],
            [
                cb.Build(
                    """I wandered around the post, knowing that I would run into Greltza sooner rather than later.  I
                        just hoped that I saw her before she saw me.""",
                    "szalk-7-wandering",
                    setFlags=["DESERT_KNIGHT_BACKUP"]
                )
            ]
        )
        pb.Build(
            "szalk-7-persuasion-failed",
            [
                "\"State your business,\" the guard commanded."
                "\"I am a wanderer who wishes to trade wares.  Is that not what this place is for?\" I responded."
                """The guard looked suspicious of me.  "No wanderers in town until tomorrow," he said. "We've had...
                    reports." """
            ],
            [
                cb.Build(
                    """Seeing that I would never get in this way, I returned to Desert Knight.  We would have to wait
                        until nightfall.""",
                    "szalk-7-night")
            ]
        )
        pb.Build(
            "szalk-7-night",
            [
                """We waited throughout the hot day in some improvised shade made by draping our bedrolls over a gnarled
                    cactus.  Once night fell, Desert Knight and I prepared ourselves."""
                """The plan was that Desert Knight and I would climb over the walls under cover of night, find Greltza,
                    and deal with her as quietly as possible."""
            ],
            [
                cb.Build(
                    "We scaled the bare stone walls easily, and slipped into the trading post.",
                    "szalk-7-wandering",
                    SkillCheckRequired (Might, Sneaking, 1, AlternatePage "szalk-7-sneaking-failed"),
                    setFlags=["DESERT_KNIGHT_BACKUP"])
            ]
        )
        pb.Build(
            "szalk-7-sneaking-failed",
            [
                """We scaled the bare stone walls easily, and dropped silently down to the other side, where we seemed
                    to be behind some kind of public house.  Our luck immediately soured, as a drunk patron stumbled out
                    the back door to vomit."""
                """Desert Knight and I stayed as still as we could, but it was too late.  She ran back inside, our
                    presence driving all nausea from her."""
                """We heard the people inside readying arms.  This trading post seemed to be infested with bandits, and
                    each of them would love to be the one to lay Desert Knight to their final rest."""
            ],
            [
                cb.Build(
                    """We ran through a side alley, blades out.  It came to fighting in a few areas, but we were able
                        to push our way through without having to hurt anybody too badly.""",
                    "szalk-7-sneaking-failed-2",
                    SkillCheckRequired (Might, Combat, 3, AttributeDamage))
                cb.Build(
                    "We ran through a side alley as quietly as we could, making sure to loop back occasionally to throw
                        off pursuers.",
                    "szalk-7-sneaking-failed-2",
                    SkillCheckRequired (Might, Sneaking, 3, AttributeDamage))
                cb.Build(
                    """We ran off through a side alley as I prepared a flashbang spell.  Once we were in the town
                        square I set it off, blinding both our original purshuers and those who had joined them after
                        the hue and cry.""",
                    "szalk-7-sneaking-failed-2",
                    SkillCheckRequired (Will, Ritual, 3, AttributeDamage))
            ]
        )
        pb.Build(
            "szalk-7-wandering",
            [
                """I wandered the trading post, keeping a sharp eye out for Greltza or any of her crew.  Since they
                    would recognize me on sight I made sure to stick to back alleys.  After a few minutes, I spotted
                    her!  She was heading down to the dock, and I followed her from a distance.  There was a boat moored
                    there, and she got on and went belowdecks."""
            ],
            [
                cb.Build(
                    """Then I saw her lieutenant board the ship, towing the [[Deskite|Deskites]] child behind him.  I
                        had to get on this boat, now!  I walked down to the dock, and boarded the ship.  Good thing it
                        seemed like her bandits were staying on land, and she and her lieutenant were the only ones on
                        the ship who would recognize me.""",
                    "szalk-7-ship",
                    Flags (["DESKITE_CHILD"; "~CHILD_RETRIEVED"; "~DESERT_KNIGHT_BACKUP"], Automatic))
                cb.Build(
                    """Then I saw her lieutenant board the ship, towing the [[Deskite|Deskites]] child behind him.  I
                        had to get on this boat, now!  I walked down to the dock, and saw the Desert Knight waiting
                        there in hiding.  I pointed to the ship I was boarding, and they nodded back.  As I boarded the
                        ship, I was thankful that it seemed like her bandits were staying on land, and she and her
                        lieutenant were the only ones on the ship who would recognize me.""",
                    "szalk-7-ship",
                    Flags (["DESKITE_CHILD"; "~CHILD_RETRIEVED"; "DESERT_KNIGHT_BACKUP"], Automatic))
                cb.Build(
                    """Then I saw her lieutenant board the ship, holding a box that looked the right size to hold a
                        heartseed.  I had to get on this boat, now!  I walked down to the dock, and boarded the ship.
                        Good thing it seemed like her bandits were staying on land, and she and her lieutenant were the
                        only ones on the ship who would recognize me.""",
                    "szalk-7-ship",
                    Flags (["~DESKITE_CHILD"; "~DESERT_KNIGHT_BACKUP"], Automatic))
                cb.Build(
                    """Then I saw her lieutenant board the ship, holding a box that looked the right size to hold a
                        heartseed.  I had to get on this boat, now!  I walked down to the dock, and saw the Desert
                        Knight waiting there in hiding.  I pointed to the ship I was boarding, and they nodded back.  As
                         I boarded the ship, I was thankful that it seemed like her bandits were staying on land, and
                         she and her lieutenant were the only ones on the ship who would recognize me.""",
                    "szalk-7-ship",
                    Flags (["~DESKITE_CHILD"; "DESERT_KNIGHT_BACKUP"], Automatic))
            ])
        pb.Build(
            "szalk-7-sneaking-failed-2",
            [
                """We ran towards the docks.  I didn't want to take the chance that Greltza could sail away at the sound
                    of this commotion, and if she wasn't there we could still check other parts of town, as long as we
                    could stay ahead of the mob chasing us."""
                """And it's a good thing we ran down to the docks, too.  Up ahead, we could make out Greltza boarding a
                    moored ship on the gangplank."""
            ],
            [
                cb.Build(
                    """Then I saw her lieutenant board the ship, towing the [[Deskite|Deskites]] child behind him.  I
                        had to get on this boat, now!  Desert Knight and I sprinted down the dock, and up the gangplank.
                        Things had gone a bit more south than expected, but neither Desert Knight nor I would give up
                        when the life of a child was concerned.""",
                    "szalk-7-ship-fighting",
                    Flags (["DESKITE_CHILD"; "~CHILD_RETRIEVED"; "DESERT_KNIGHT_BACKUP"], Automatic))
                cb.Build(
                    """Then I saw her lieutenant board the ship, holding a box that looked the right size to hold a
                        heartseed.  I had to get on this boat, now!  Desert Knight and I sprinted down the dock, and up
                        the gangplank. Things had gone a bit more south than expected, but neither Desert Knight nor I
                        would give up when it meant the life of an entire town.""",
                    "szalk-7-ship-fighting",
                    Flags (["~DESKITE_CHILD"; "DESERT_KNIGHT_BACKUP"], Automatic))
            ]
        )


        pb.Build(
            "middle",
            ["You made it closer..."],
            [cb.Build("Go easter", "end", SkillCheckRequired (Will, Persuasion, 20, AlternatePage "middle2"))])
        pb.Build(
            "middle2",
            ["You made it closer, but then you fucked up."],
            [
                cb.Build("Go eastest", "middle2", SkillCheckRequired (Might, Combat, 20, AttributeDamage), ["YouFuckedItUpViolently"])
                cb.Build("Go eastest, but pay", "middle2", Bribe 15, ["YouFuckedItUpPayedly"; "TESTHARNESSENABLED"])
                cb.Build("Beat the game quickly", "middle3", Flags (["~YouFuckedItUpViolently"; "~YouFuckedItUpPayedly"], Automatic))
                cb.Build("Beat the game because you paid", "middle3", Flags (["~YouFuckedItUpViolently"; "YouFuckedItUpPayedly"], Automatic))
                cb.Build("Beat the game because you fought", "middle3", Flags (["YouFuckedItUpViolently"; "~YouFuckedItUpPayedly"], Automatic))
                cb.Build("Beat the game already!", "middle3", Flags (["YouFuckedItUpViolently"; "YouFuckedItUpPayedly"], Automatic))
            ]
        )
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
    
    for part in List.collect (fun cont -> Modals.parseLine cont.Description) kvp.Value.Continuations do
        match part with
        | Modals.Str _ -> ()
        | Modals.Link link ->
            if not (Map.containsKey link.LinkName Modals.modals) then
                printfn "Page %s has an invalid modal link: %s" kvp.Value.Name link.LinkName