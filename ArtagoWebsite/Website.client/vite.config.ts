import { defineConfig } from 'vite';
import fs from 'fs';
import path from 'path'; 
import child_process from 'child_process';

const baseFolder = process.env.APPDATA !== undefined && process.env.APPDATA !== '' ? `${process.env.APPDATA}/ASP.NET/https` : `${process.env.HOME}/.aspnet/https`
const certificateName = "Website.client";
const certFilePath = path.join(baseFolder, `${ certificateName }.pem`);
const keyFilePath = path.join(baseFolder, `${ certificateName }.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync(
        'dotnet',
        ['dev-certs', 'https', '--export-path', certFilePath, '--format', 'pem', '--no-password',],
        { stdio: 'inherit', })
        .status
    )
    {
        throw new Error(`${certFilePath}`);
    }
}

export default defineConfig({
    server: {
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
