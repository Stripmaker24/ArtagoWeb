import { Box } from '@mui/material'
import Navbar from './components/Navbar/navbar';
import './App.css'
import React from 'react'

function App() {
  return (
      <>
        <Navbar></Navbar>
        <Box id="main_background">
              <Box sx={{ backgroundColor: 'rgba(21, 39, 25, 0.5)', position: 'fixed', height: '100%', width:'100%'}}></Box>
        </Box>
    </>
  )
}

export default App
