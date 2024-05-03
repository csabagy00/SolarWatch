import { Link } from "react-router-dom"
import "./NavBar.css"


function NavBar({ current, setToken, token }){
  return(
    <nav className="navbar">
      <Link to={token == "" ? '/' : '/solar-watch'}>
        <button className='main'>SolarWatch</button>
      </Link>
      <div className="buttons">
      { token == "" ? 
      <>
        <Link to="/login">
          <button className="logreg">Login</button>
        </Link>
        <Link to="/registration">
          <button className="logreg">Register</button>
        </Link>
      </>
      : 
      <>
        <Link to='/'>
          <button className="logreg" onClick={() => setToken("")}>Logout</button>
        </Link>
      </>
      }
      </div>
    </nav>
  )
}

export default NavBar
