pipeline {
    agent any
	environment {
		VERSION = "1.0"
	}
    stages {
        stage('Build') {
            steps {
                echo 'building'
                echo 'https://github.com/StefanMihaylov/MihaylovWeb.git'
				echo "bulding version ${VERSION}.${BUILD_NUMBER}.${GIT_COMMIT}"
            }
        }
         stage('Test') {
			when {
				expression {
					env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'dev' 
				}
			}
            steps {
                echo 'testing'
            }
        }
         stage('deploy') {
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
