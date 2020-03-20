import * as React from "react";
import {Category} from "../../store/Games";
import {Button, ToggleButton, ToggleButtonGroup} from "react-bootstrap";

interface CategorySelectionProps {
    categories: Category[];
    play: (categories: number[]) => void;
}

interface CategorySelectionState {
    selectedCategories: number[];
}

export default class CategorySelection extends React.Component<CategorySelectionProps, CategorySelectionState> {
    constructor(props: CategorySelectionProps) {
        super(props);
        this.state = {selectedCategories: []};

        this.toggleCategory = this.toggleCategory.bind(this);
        this.selectCategories = this.selectCategories.bind(this);
    }

    private toggleCategory(val: any) {
        const selection = val as number[];
        this.setState({selectedCategories: selection});
    };
    
    private selectCategories() {
        const selectedCategories = this.state.selectedCategories;
        if (selectedCategories.length >= 1){
            this.props.play(selectedCategories);
        }
    }

    public render() {
        return (
            <div>
                <h3>Select categories</h3>
                <br/>
                <div style={{textAlign: 'center'}}>
                    <ToggleButtonGroup vertical type="checkbox" onChange={this.toggleCategory} style={{margin: '0 auto'}}>
                        {this.props.categories.map(c =>
                            <ToggleButton value={c.categoryId!!} size="lg" key={c.categoryId!!}>{c.name}</ToggleButton>
                        )}
                    </ToggleButtonGroup>
                </div>
                <br/>
                <Button style={{float: 'right'}} variant="outline-primary" onClick={this.selectCategories}>Play</Button>
            </div>
        );
    }
}