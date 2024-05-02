import './css/MainPage.css'

function MainPage({ token }){

  return(
    <div className='maindiv'>
      {token == "" ? 
      (
        <div className="notloggedin">
          <p className="mustlogin">To use the feature of the site, please Log in or Register.</p>
        </div>
      ) :
      (
        <div>

        </div>
      )}
    </div>
  )

}

export default MainPage