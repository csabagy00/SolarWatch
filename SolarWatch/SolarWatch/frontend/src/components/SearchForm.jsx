import '../pages/css/Form.css'

function SearchForm({ setCity, setDate, city, date, handleSubmit}){

return(
  <div className="formDiv">
    <form onSubmit={handleSubmit}>
      <div className="regInput">
        <label htmlFor="city">City name:</label>
        <input type='text' id="city" placeholder='budapest' value={city} onChange={(e) => setCity(e.target.value)}/> 
      </div>
      <div className="regInput">
        <label htmlFor="date">Date:</label>
        <input type='text' id="date" placeholder='2024-05-01' value={date} onChange={(e) => setDate(e.target.value)}/> 
      </div>
      <button type='submit'>Search</button>
    </form>
  </div>
)
}


export default SearchForm