import { useState } from 'react'
import SearchForm from '../components/SearchForm'
import SunData from '../components/SunData'
import './css/MainPage.css'

function MainPage({ token }){
  const [city, setCity] = useState("")
  const [date, setDate] = useState("")
  const [data, setData] = useState(null)

  const handleSubmit = async (e) => {
    e.preventDefault()
    setCity("")
    setDate("")

    try {
      const response = await fetch(`/api/Sun/Get?city=${city}&date=${date}`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      const result = await response.json()

      setData(result)
      console.log(result)

    } catch (error) {
      console.error('Error submitting GET request', error)
    }
  }

  return(
    <div className='maindiv'>
      <div>
        <SearchForm setCity={setCity} setDate={setDate} city={city} date={date} handleSubmit={handleSubmit}/>
        {data != null ? <SunData data={data}/> : <></>}
      </div>
    </div>
  )

}

export default MainPage