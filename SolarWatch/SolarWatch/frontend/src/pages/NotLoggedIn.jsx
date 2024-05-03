import './css/MainPage.css'

function NotLoggedIn(){

  return(
    <div className='maindiv'>
      <div className="notloggedin">
      <p className="mustlogin">To use the feature of the site, please Log in or Register.</p>
      </div>
    </div>
  )
}

export default NotLoggedIn