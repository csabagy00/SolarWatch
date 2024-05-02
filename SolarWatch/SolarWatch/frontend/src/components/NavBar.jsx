import { Link } from "react-router-dom"
import "./NavBar.css"


function NavBar({ current }){
  return(
    <nav className="navbar">
      <Link to='/'>
        <button className='main'>SolarWatch</button>
      </Link>
      <div className="buttons">
        <Link to="/login">
          <button className="logreg">Login</button>
        </Link>
        <Link to="/registration">
          <button className="logreg">Register</button>
        </Link>
      </div>
    </nav>
  )
}

export default NavBar
