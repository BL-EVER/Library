$clientId = "backend" 
$clientSecret = "wohzlqvIX4AH416X3mlxrFfCzldggId2"
$realm = "app"
$serverUrl = "http://localhost:8080"
$username = "admin"
$password = "admin"

# Create a token request
$body = @{
    client_id     = $clientId
    client_secret = $clientSecret
    grant_type    = "password"
    username      = $username
    password      = $password
}

# Convert body to form data
$body = $body.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" } 
$body = [String]::Join("&", $body)


# Make the token request
$response = Invoke-RestMethod -Method Post -Uri "$serverUrl/realms/$realm/protocol/openid-connect/token" -ContentType "application/x-www-form-urlencoded" -Body $body

# Get the access token from the response
$accessToken = "Bearer " + $response.access_token

# Copy the access token to the clipboard
Set-Clipboard -Value $accessToken
