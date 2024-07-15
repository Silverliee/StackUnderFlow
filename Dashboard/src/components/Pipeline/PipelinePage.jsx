import React from 'react';
import { Button, Typography, Container, Grid, Paper } from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import '../../styles/PipelinePage.css';

const PipelinePage = () => {
    return (
        <Container className="pipeline-container">
            <div className="header">
                <Typography variant="h6" className="header-item">Pipeline</Typography>
                <Typography variant="h6" className="header-item">Needs</Typography>
                <Typography variant="h6" className="header-item">Jobs 5</Typography>
                <Typography variant="h6" className="header-item">Tests 0</Typography>
            </div>
            <div className="group-jobs">
                <Typography variant="body1">Group jobs by</Typography>
                <Button variant="contained" className="group-button">Stage</Button>
                <Button variant="contained" className="group-button">Job dependencies</Button>
            </div>
            <Grid container spacing={2} className="stages">
                <Grid item xs={3}>
                    <Paper className="stage">
                        <Typography variant="h6">Build</Typography>
                        <div className="job">
                            <CheckCircleIcon className="icon" />
                            <Typography>build</Typography>
                        </div>
                    </Paper>
                </Grid>
                <Grid item xs={3}>
                    <Paper className="stage">
                        <Typography variant="h6">Test</Typography>
                        <div className="job">
                            <CheckCircleIcon className="icon" />
                            <Typography>test1</Typography>
                        </div>
                        <div className="job">
                            <CheckCircleIcon className="icon" />
                            <Typography>test2</Typography>
                        </div>
                    </Paper>
                </Grid>
                <Grid item xs={3}>
                    <Paper className="stage">
                        <Typography variant="h6">Deploy</Typography>
                        <div className="job">
                            <CheckCircleIcon className="icon" />
                            <Typography>auto-deploy</Typography>
                        </div>
                    </Paper>
                </Grid>
                <Grid item xs={3}>
                    <Paper className="stage">
                        <Typography variant="h6">Production</Typography>
                        <div className="job">
                            <CheckCircleIcon className="icon" />
                            <Typography>deploy to prod</Typography>
                        </div>
                    </Paper>
                </Grid>
            </Grid>
        </Container>
    );
}

export default PipelinePage;
