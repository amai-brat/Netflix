// noinspection JSUnusedGlobalSymbols
/* eslint-disable no-undef */

import {defineConfig} from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
        plugins: [react()],
        server: {
            watch: {
                usePolling: true,
            },
            strictPort: true,
            host: process.env.VITE_HOST || "0.0.0.0",
            port: process.env.VITE_PORT || 5173,
            // hmr: {
            //   clientPort: process.env.VITE_CLIENT_PORT || null
            // },
        },
        test: {
            environment: 'jsdom',
            globals: true,
            setupFiles: './setupTests.js'
        },
        resolve: {
            preserveSymlinks: true,
        }
    },
)