module rec Wanderer.Model

open Wanderer.Modals
open Wanderer.Pages
open Wanderer.Skills

type InProgressCharacter = {
    Might : int
    Will : int
    HighSkill : Skill
    LowSkill : Skill
}

type Character = {
    Might : int
    Will : int
    Persuasion : int
    Combat : int
    Ritual : int
    Sneaking : int
    Wounds : int
    Stress : int
    Muld : int
}
with
    static member GetAttr attr character =
        match attr with
        | Might -> character.Might
        | Will -> character.Will

    static member GetSkill skill character =
        match skill with
        | Persuasion -> character.Persuasion
        | Combat -> character.Combat
        | Ritual -> character.Ritual
        | Sneaking -> character.Sneaking

type ActiveGameState = {
    Character : Character
    Page : Page
    History : string list
    FullHistory : string list
    Flags : string Set
}

type SavedGameState = {
    Character : Character
    PageName : string
    History : string list
    FullHistory : string list
    Flags : string list
}

type Message =
    | StartCharacterCreation
    | UpdateCharacter of InProgressCharacter
    | StartGame
    | LoadGame of ActiveGameState
    | Flip of Continuation
    | ShowModal of Modal
    | CloseModal
    | ShowFullHistory

type Model =
    | SplashScreen
    | CharacterCreation of InProgressCharacter
    | ActiveGame of ActiveGameState
    | GameWithResult of ActiveGameState * Skills.RollResult
    | Modal of Modal * Model
    | History of ActiveGameState