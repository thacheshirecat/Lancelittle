import React, { useState, useEffect } from 'react'

const CharacterList = () => {
    const [characters, setCharacters] = useState([{}])

    const getCharacters = async () => {
        try {
            const response = await fetch(
                'https://localhost:5001/Character/getall',
                { mode: 'cors' }
            )
            const data = await response.json()
            setCharacters(data.data)
        } catch (error) {
            console.log(error)
        }
    }

    useEffect(() => {
        getCharacters()
    }, [])

    return (
        <>
            <h1>Characters</h1>
            {characters.map((character) => {
                let characterClass = 'None'
                let relic = 'None'
                if (character.class) {
                    characterClass = character.class.name
                }
                if (character.relic) {
                    relic = character.relic.name
                }
                return (
                    <div key={character.id}>
                        <h3>{character.name}</h3>
                        <h4>Class: {characterClass}</h4>
                        <h4>Holy Relic: {relic}</h4>
                        <h5>HP: {character.hitPoints}</h5>
                        <h5>Strongth: {character.strength}</h5>
                        <h5>Smort: {character.intelligence}</h5>
                        <h5>Fast: {character.dexterity}</h5>
                    </div>
                )
            })}
        </>
    )
}

export default CharacterList
