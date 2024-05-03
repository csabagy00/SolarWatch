import React from 'react'
import { BrowserRouter, Routes, Route, Navigate} from 'react-router-dom'
import { useState } from 'react'
import Registration from './pages/Registration'
import NavBar from './components/NavBar'
import './App.css'
import Login from './pages/Login'
import MainPage from './pages/MainPage'
import NotLoggedIn from './pages/NotLoggedIn'

function App() {
  const [token, setToken] = useState("")

  return (
    <BrowserRouter>
      <NavBar token={token} setToken={setToken}/>
        <Routes>
          <Route path='/' element={<NotLoggedIn />}/>
          <Route path='/solar-watch' element={token != "" ? <MainPage token={token}/> : <Navigate to="/login"/>}/>
          <Route path='/login' element={<Login setToken={setToken} token={token}/>}/>
          <Route path='/registration' element={<Registration />}/>
        </Routes>
    </BrowserRouter>
  )
}

export default App
