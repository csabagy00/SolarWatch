import React from 'react'
import { BrowserRouter, Routes, Route} from 'react-router-dom'
import { useState } from 'react'
import Registration from './pages/Registration'
import NavBar from './components/NavBar'
import './App.css'
import Login from './pages/Login'
import MainPage from './pages/MainPage'

function App() {
  const [token, setToken] = useState("")
  const [current, setCurrent] = useState("")

  return (
    <BrowserRouter>
      <NavBar current={current}/>
        <Routes>
          <Route path='/' element={<MainPage token={token}/>}/>
          <Route path='/login' element={<Login setToken={setToken}/>}/>
          <Route path='/registration' element={<Registration />}/>
        </Routes>
    </BrowserRouter>
  )
}

export default App
