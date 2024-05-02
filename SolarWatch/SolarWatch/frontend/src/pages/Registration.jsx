import { useState } from "react"
import './css/Form.css'

function Registration() {
  const [username, setUsername] = useState("")
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")

  const handleSubmit = async (e) => {
    e.preventDefault();
   
      try {
        const response = await fetch('/api/Auth/Register', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            username: username,
            email: email,
            password: password
          })
        })

        if(response.ok){
          setUsername("")
          setEmail("")
          setPassword("")            
          console.log("Registration successful");
        }

      } catch (error) {
        console.error('Error submitting registration:', error);
      }
    
  }

  return(
    <div className="formDiv">
      <form onSubmit={handleSubmit}>
        <div className="regInput">
          <label htmlFor="username">Username:</label>
          <input type="text" id="username" value={username} onChange={(e) => setUsername(e.target.value)} required/>
        </div>
        <div className="regInput">
          <label htmlFor="email">Email:</label>
          <input type="email" id="email" value={email} onChange={(e) => setEmail(e.target.value)} required/>
        </div>
        <div className="regInput">
          <label htmlFor="password">Password:</label>
          <input type="password" id="password" value={password} onChange={(e) => setPassword(e.target.value)} required/>
        </div>
        <button type="submit">Register</button>
      </form>
    </div>
  )
}

export default Registration