import { defineConfig } from 'vite';
import tailwindcss from '@tailwindcss/vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [tailwindcss(), plugin()],
    preview: {
        port: 4173,
        strictPort: true,
    },
    server: {
        port: 64461,
        strictPort: true,
        host: true,
        origin: "http://0.0.0.0:64461",
    },
})