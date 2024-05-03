import './css/SunData.css'

function SunData({ data }){

return(
  <div className="card">
    <div className="cardinfo">
      <div className='infodiv'>
        <label htmlFor="cityname">City:</label>
        <p className="pinfo">{data.city}</p>
      </div>
      <div className='infodiv'>
        <label htmlFor="dateof">Date:</label>
        <p className="pinfo">{data.date}</p>
      </div>
    </div>
    <div className='sundata'>
      <div className="sun">
        <img src="../src/assets/sunrise.png" alt="Image" width="150" height="150"/>
        <p className="suntime">{data.sunrise}</p>
      </div>
      <div className="sun">
        <img src="../src/assets/sunset.png" alt="Image" width="150" height="150"/>
        <p className="suntime">{data.sunset}</p>
      </div>
    </div>
  </div>
  )
}

export default SunData