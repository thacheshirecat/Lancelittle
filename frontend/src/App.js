import logo from './logo.svg'
import './App.css'
import CharacterList from './Components/CharacterList'

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo" />
                <CharacterList />
            </header>
        </div>
    )
}

export default App
