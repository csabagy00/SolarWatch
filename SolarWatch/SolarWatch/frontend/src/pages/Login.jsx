import { useState } from 'react'
import './css/Form.css'

function Login({ setToken }){
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch('/api/Auth/Login', {
        method: 'POST',
        headers: {
          'Content-Type':'application/json'
        },
        body: JSON.stringify({
          email: email,
          password: password
        })
      })

      if(response.ok){
        setEmail("")
        setPassword("")
        console.log("Login successful")
        const result = await response.json()
        setToken(result.token)
      }
      
    } catch (error) {
      console.error('Error submitting login:', error);
    }
  }

  return(
    <div className='formDiv'>
      <form onSubmit={handleSubmit}>
        <div className='regInput'>
          <label htmlFor='email'>Email:</label>
          <input type='text' id='email' value={email} onChange={(e) => setEmail(e.target.value)} required/>
        </div>
        <div className='regInput'>
          <label htmlFor='password'>Password:</label>
          <input type='password' id='password' value={password} onChange={(e) => setPassword(e.target.value)} required/>
        </div>
        <button type='submit'>Login</button>
      </form>
    </div>
  )

}

export default Login