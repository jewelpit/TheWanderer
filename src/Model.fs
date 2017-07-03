module Wanderer.Model

type InProgressCharacter = {
    Might : int
    Will : int
    Persuasion : int
    Combat : int
    Ritual : int
    Sneaking : int
}

type Model =
    | CharacterCreation of InProgressCharacter

type Message =
    | UpdateCharacter of InProgressCharacter