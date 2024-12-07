import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import React from 'react'
import { ThemeProvider } from '@emotion/react'
import Theme from './utilities/Theme.ts';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ThemeProvider theme={Theme}>
            <App />
        </ThemeProvider>
  </StrictMode>,
)
