
pipeline {
	// def app
	
    agent {
		label 'docker-agent-dind'
	}
	environment {
		VERSION = "1.0"
		// DOCKER_CREDENTIALS = credentials('')
	}
	stages {
	    stage('Clone repository') {
			steps{
				checkout scm
			}		    
		}
        stage('Build') {
            steps {
                echo 'building'
                echo 'https://github.com/StefanMihaylov/MihaylovWeb.git'
				echo "bulding version ${VERSION}.${BUILD_NUMBER}.${GIT_COMMIT}"
				
				sh 'ls'
				sh 'docker info'
				// app = docker.build("getintodevops/hellonode")				
            }
        }
         stage('Test') {
			when {
				expression {
					env.BRANCH_NAME != 'master' || env.BRANCH_NAME == 'dev' 
				}
			}
            steps {
                echo 'testing'
				app.inside {
						sh 'echo "Tests passed"'
					}
            }
        }
		stage('Push image') {
			steps {
					echo 'Push image'
					// docker.withregistry('https://registry.hub.docker.com', 'docker-hub-credentials') {
					// app.push("${env.build_number}")
					//	app.push("latest")
					//}
			}
		}
        stage('Deploy') {
            steps {
                echo 'deploying'
            }
        }   
    }
	
	//post{
	//	always {
	//	}		
	//	success {
	//	}		
	//	failure {
	//	}
	//}
}
