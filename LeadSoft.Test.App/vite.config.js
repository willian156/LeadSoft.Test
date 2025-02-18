import { defineConfig } from 'vite';
import tailwindcss from '@tailwindcss/vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [tailwindcss(), plugin()],
    server: {
        port: 64461,
    }
})