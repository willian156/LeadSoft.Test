import { useState } from 'react';
import Login from './pages/Login';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import ProcessedItens from './pages/ProcessedItens';
import UserList from './pages/UserList';
import CreateUser from './pages/CreateUser';
import StartProcess from './pages/StartProcess';

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/ProcessedItens" element={<ProcessedItens/>}/>
        <Route path="/UserList" element={<UserList/>}/>
        <Route path="/CreateUser" element={<CreateUser/>}/>
        <Route path="/StartProcess" element={<StartProcess/>}/>
      </Routes>
    </BrowserRouter>
  )
}

export default App
