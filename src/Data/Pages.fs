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
                    "eresh-3-spotted_in_eresh_2")
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
            ])
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
            ])
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
            ])
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
            ])
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
            ])
        pb.Build(
            "eresh-3-magic",
            [
                """With a flash brighter than the sun and a sound louder than any I had heard before, the five bandits
                    dropped to the ground, clutching their heads.  That would teach them to underestimate the people
                    they rob."""
            ],
            [
                cb.Build("I continued on my way.", "eresh-4")
            ])

        pb.Build(
            "eresh-4",
            [
                """With the bandits no longer an issue, I decided that it was time for a small rest.  After some more
                    cold pork and a few hours' sleep under a drooping tree, I continued on as the sun broke over the
                    mountain ridge to the east."""
            ],
            [
                cb.Build("It was two more days until the next obstacle in my path.", "eresh-5")
            ])

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
                    "eresh-7",
                    setFlags=["LOW_ROAD"])
            ])

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
            ])

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
            ])

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
                """I was able to find out that his name was Kherek, and he had been making his way from the
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
                    Bribe 20)
            ])
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
            ])

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
                    having trouble conceiving, and when the child was finally born it was hailed as a small miracle.
                    Now, to have the child stolen from them..."""
            ],
            [
                cb.Build(
                    "Greltza will pay.  I will return your child to you.", "eresh-9-climb", setFlags=["DESKITE_CHILD"])
            ])
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
            ])
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
            ])
        pb.Build(
            "eresh-9-sneaking-talking",
            [
                """ "Leave us, uplander!" the closest Deskite said in the characteristic Deskite drawl.  "We have
                    suffered enough this day." """
                """With a little gentle prodding, I was able to find out that Greltza and her crew had indeed come
                    through this passage, and taken the child that this hexad was raising.  The breeding pair in the
                    hexad had been having trouble conceiving, and when the child was finally born it was hailed as a
                    small miracle. Now, to have the child stolen from them..."""
            ],
            [
                cb.Build(
                    "Greltza will pay.  I will return your child to you.", "eresh-9-climb", setFlags=["DESKITE_CHILD"])
            ])
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
                cb.Build("After taking a moment to rest, I pressed on.", "eresh-10")
            ])

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
            ])
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
            ])
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
            ])
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
            ])
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
            ])
        pb.Build(
            "eresh-10-bribed",
            [
                """When I finished counting out the shells, the bandits went back the way they came, deeper into the
                    mines.  Perhaps in a few months I would be called back to deal with these four.  In the meanwhile,
                    though, I had more important work."""
            ],
            [
                cb.Build("I rode my beetle through the open door.", "szalk-1-bribed")
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